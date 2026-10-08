using FluentValidation;
using MediatR;
using Modules.User.Contracts;
using Modules.WorkItems.Application.Abstractions;
using Modules.WorkItems.Application.WorkItems;
using Modules.WorkItems.Application.WorkItems.CreateWorkItem;
using Modules.WorkItems.Application.WorkItems.GetWorkItemsByAssignee;
using Modules.WorkItems.Domain.WorkItems;
using NSubstitute;
using NUnit.Framework;

namespace Modules.WorkItems.UnitTests.Application;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public sealed class WorkItemHandlersTests
{
    private readonly ISender _sender;
    private readonly IWorkItemRepository _repository;
    private readonly IWorkItemQueries _queries;

    public WorkItemHandlersTests()
    {
        _sender = Substitute.For<ISender>();
        _repository = Substitute.For<IWorkItemRepository>();
        _queries = Substitute.For<IWorkItemQueries>();
    }

    [Test]
    public async Task Create_WhenAssigneeExists_PersistsAndReturnsWorkItem()
    {
        var assigneeId = Guid.NewGuid();
        _sender.Send(
                Arg.Is<UserExistsQuery>(query => query.UserId == assigneeId),
                Arg.Any<CancellationToken>())
            .Returns(true);
        var handler = new CreateWorkItemCommandHandler(_sender, _repository);

        var result = await handler.Handle(
            new CreateWorkItemCommand(" Work ", " Description ", assigneeId),
            CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(CreateWorkItemStatus.Created));
            Assert.That(result.WorkItem, Is.Not.Null);
            Assert.That(result.WorkItem!.Name, Is.EqualTo("Work"));
            Assert.That(result.WorkItem.Description, Is.EqualTo("Description"));
            Assert.That(result.WorkItem.AssigneeId, Is.EqualTo(assigneeId));
        });
        await _repository.Received(1).AddAsync(
            Arg.Is<WorkItem>(item => item.AssigneeId == assigneeId),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Create_WhenAssigneeDoesNotExist_ReturnsUserNotFoundWithoutPersisting()
    {
        var assigneeId = Guid.NewGuid();
        _sender.Send(Arg.Any<UserExistsQuery>(), Arg.Any<CancellationToken>())
            .Returns(false);
        var handler = new CreateWorkItemCommandHandler(_sender, _repository);

        var result = await handler.Handle(
            new CreateWorkItemCommand("Work", "Description", assigneeId),
            CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(CreateWorkItemStatus.UserNotFound));
            Assert.That(result.WorkItem, Is.Null);
        });
        await _repository.DidNotReceive().AddAsync(
            Arg.Any<WorkItem>(),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Validator_WithBlankName_ReturnsValidationFailure()
    {
        var validator = new CreateWorkItemCommandValidator();

        var result = await validator.ValidateAsync(
            new CreateWorkItemCommand(" ", "Description", Guid.NewGuid()));

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Some.Property("ErrorMessage").EqualTo("Work item name must not be empty."));
    }

    [Test]
    public async Task GetByAssignee_ReturnsQueryResults()
    {
        var assigneeId = Guid.NewGuid();
        IReadOnlyList<WorkItemDto> expected =
        [
            new(Guid.NewGuid(), "One", "First", assigneeId),
            new(Guid.NewGuid(), "Two", "Second", assigneeId)
        ];
        _queries.GetByAssigneeAsync(assigneeId, Arg.Any<CancellationToken>())
            .Returns(expected);
        var handler = new GetWorkItemsByAssigneeQueryHandler(_queries);

        var result = await handler.Handle(
            new GetWorkItemsByAssigneeQuery(assigneeId),
            CancellationToken.None);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetByAssignee_WhenNoMatches_ReturnsEmptyCollection()
    {
        var assigneeId = Guid.NewGuid();
        _queries.GetByAssigneeAsync(assigneeId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<WorkItemDto>());
        var handler = new GetWorkItemsByAssigneeQueryHandler(_queries);

        var result = await handler.Handle(
            new GetWorkItemsByAssigneeQuery(assigneeId),
            CancellationToken.None);

        Assert.That(result, Is.Empty);
    }
}
