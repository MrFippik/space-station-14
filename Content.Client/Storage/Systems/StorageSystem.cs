﻿using Content.Client.Animations;
using Content.Shared.Storage;

namespace Content.Client.Storage.Systems;

// TODO kill this is all horrid.
public sealed class StorageSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<AnimateInsertingEntitiesEvent>(HandleAnimatingInsertingEntities);
    }

    /// <summary>
    /// Animate the newly stored entities in <paramref name="msg"/> flying towards this storage's position
    /// </summary>
    /// <param name="msg"></param>
    public void HandleAnimatingInsertingEntities(AnimateInsertingEntitiesEvent msg)
    {
        var store = GetEntity(msg.Storage);

        if (!HasComp<ClientStorageComponent>(store))
            return;

        TryComp(store, out TransformComponent? transformComp);

        for (var i = 0; msg.StoredEntities.Count > i; i++)
        {
            var entity = GetEntity(msg.StoredEntities[i]);
            var initialPosition = msg.EntityPositions[i];
            if (EntityManager.EntityExists(entity) && transformComp != null)
            {
                ReusableAnimations.AnimateEntityPickup(entity, GetCoordinates(initialPosition), transformComp.LocalPosition, msg.EntityAngles[i], EntityManager);
            }
        }
    }
}
