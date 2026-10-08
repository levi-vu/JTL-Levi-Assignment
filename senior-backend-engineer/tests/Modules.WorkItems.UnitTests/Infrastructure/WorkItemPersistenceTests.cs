using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.WorkItems.Domain.WorkItems;
using Modules.WorkItems.Infrastructure.Persistence;
using NUnit.Framework;

namespace Modules.WorkItems.UnitTests.Infrastructure;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public sealed class WorkItemPersistenceTests
{
    private readonly string _databaseName;
    private readonly InMemoryDatabaseRoot _databaseRoot;

    public WorkItemPersistenceTests()
    {
        _databaseName = Guid.NewGuid().ToString();
        _databaseRoot = new InMemoryDatabaseRoot();
    }

    [Test]
    public async Task AddAsync_PersistsAllAggregateFields()
    {
        await using var dbContext = CreateDbContext();
        var repository = new WorkItemRepository(dbContext);
        var assigneeId = Guid.NewGuid();
        var workItem = WorkItem.Create("Name", "Description", assigneeId);

        await repository.AddAsync(workItem, CancellationToken.None);
        dbContext.ChangeTracker.Clear();

        var persisted = await dbContext.WorkItems.SingleAsync();
        Assert.Multiple(() =>
        {
            Assert.That(persisted.Id, Is.EqualTo(workItem.Id));
            Assert.That(persisted.Name, Is.EqualTo("Name"));
            Assert.That(persisted.Description, Is.EqualTo("Description"));
            Assert.That(persisted.AssigneeId, Is.EqualTo(assigneeId));
        });
    }

    [Test]
    public async Task GetByAssigneeAsync_ReturnsOnlyMatchingProjectedItemsWithoutTracking()
    {
        await using var dbContext = CreateDbContext();
        var repository = new WorkItemRepository(dbContext);
        var matchingAssigneeId = Guid.NewGuid();
        await repository.AddAsync(
            WorkItem.Create("One", "First", matchingAssigneeId),
            CancellationToken.None);
        await repository.AddAsync(
            WorkItem.Create("Two", "Second", matchingAssigneeId),
            CancellationToken.None);
        await repository.AddAsync(
            WorkItem.Create("Other", "Ignored", Guid.NewGuid()),
            CancellationToken.None);
        dbContext.ChangeTracker.Clear();
        var queries = new WorkItemQueries(dbContext);

        var results = await queries.GetByAssigneeAsync(
            matchingAssigneeId,
            CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(2));
            Assert.That(results.Select(item => item.Name), Is.EquivalentTo(new[] { "One", "Two" }));
            Assert.That(results, Has.All.Property("AssigneeId").EqualTo(matchingAssigneeId));
            Assert.That(dbContext.ChangeTracker.Entries(), Is.Empty);
        });
    }

    [Test]
    public async Task GetByAssigneeAsync_WhenNoItemsMatch_ReturnsEmptyCollection()
    {
        await using var dbContext = CreateDbContext();
        var queries = new WorkItemQueries(dbContext);

        var results = await queries.GetByAssigneeAsync(
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.That(results, Is.Empty);
    }

    private WorkItemsDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<WorkItemsDbContext>()
            .UseInMemoryDatabase(_databaseName, _databaseRoot)
            .Options;

        return new WorkItemsDbContext(options);
    }
}
