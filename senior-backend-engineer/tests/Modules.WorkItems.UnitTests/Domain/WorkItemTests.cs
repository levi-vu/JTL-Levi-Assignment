using Modules.WorkItems.Domain.WorkItems;
using NUnit.Framework;

namespace Modules.WorkItems.UnitTests.Domain;

public sealed class WorkItemTests
{
    [Test]
    public void Create_WithValidValues_CreatesAggregateAndTrimsText()
    {
        var assigneeId = Guid.NewGuid();

        var workItem = WorkItem.Create("  Prepare report  ", "  Quarterly results  ", assigneeId);

        Assert.Multiple(() =>
        {
            Assert.That(workItem.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(workItem.Name, Is.EqualTo("Prepare report"));
            Assert.That(workItem.Description, Is.EqualTo("Quarterly results"));
            Assert.That(workItem.AssigneeId, Is.EqualTo(assigneeId));
        });
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Create_WithBlankName_Throws(string? name)
    {
        Assert.That(
            () => WorkItem.Create(name, "Description", Guid.NewGuid()),
            Throws.ArgumentException.With.Property("ParamName").EqualTo("name"));
    }

    [Test]
    public void Create_WithEmptyAssigneeId_Throws()
    {
        Assert.That(
            () => WorkItem.Create("Name", "Description", Guid.Empty),
            Throws.ArgumentException.With.Property("ParamName").EqualTo("assigneeId"));
    }
}
