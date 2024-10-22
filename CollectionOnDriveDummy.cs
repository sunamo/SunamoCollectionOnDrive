namespace SunamoCollectionOnDrive;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// class which is used to avoid the error CS8625 Cannot convert null literal to non-nullable reference type.
/// </summary>
/// <param name="logger"></param>
internal class CollectionOnDriveDummy(ILogger logger) : CollectionOnDriveBase<string>(logger)
{
    public static CollectionOnDriveDummy Instance = new CollectionOnDriveDummy(NullLogger.Instance);

    public override Task Load(bool removeDuplicates)
    {
        ThrowEx.UseNonDummyCollection();
        return Task.CompletedTask;
    }

    public override void AddWithoutSave(string t)
    {
        ThrowEx.UseNonDummyCollection();
    }

    public override Task<bool> AddWithSave(string? element)
    {
        ThrowEx.UseNonDummyCollection();
        return Task.FromResult(false);
    }
}