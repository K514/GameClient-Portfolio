using System;
using System.Collections.Generic;

namespace k514.Mono.Common
{
    public class PhysicsCollisionLayerTable : GameTable<PhysicsCollisionLayerTable, TableMetaData, GameConst.GameLayerType, PhysicsCollisionLayerTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            public List<GameConst.GameLayerType> CollisionGameLayerSet { get; private set; }
        }
    }
}