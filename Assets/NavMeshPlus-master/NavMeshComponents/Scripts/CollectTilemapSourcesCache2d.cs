/*using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace NavMeshPlus.Extensions
{
    [ExecuteAlways]
    [AddComponentMenu("Navigation/Navigation CacheTilemapSources2d", 30)]
    public class CollectTilemapSourcesCache2d : NavMeshExtension
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private NavMeshModifier _modifier;
        [SerializeField] private NavMeshModifierTilemap _modifierTilemap;

        private List<NavMeshBuildSource> _sources;
        private Dictionary<Vector3Int, int> _lookup;
        private Dictionary<TileBase, NavMeshModifierTilemap.TileModifier> _modifierMap;

        protected override void Awake()
        {
            if (_tilemap != null)
            {
                _modifier ??= _tilemap.GetComponent<NavMeshModifier>();
                _modifierTilemap ??= _tilemap.GetComponent<NavMeshModifierTilemap>();

                if (_modifierTilemap != null)
                {
                    _modifierMap = _modifierTilemap.GetModifierMap();
                }
            }

            Order = -1000;
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Tilemap.tilemapTileChanged += OnTilemapTileChangedModern;
        }

        protected override void OnDisable()
        {
            Tilemap.tilemapTileChanged -= OnTilemapTileChangedModern;
            base.OnDisable();
        }

        private void OnTilemapTileChangedModern(Tilemap tilemap, Tilemap.TileChangeData[] changedData)
        {
            if (tilemap == _tilemap && _sources != null && _lookup != null)
            {
                foreach (var change in changedData)
                {
                    Vector3Int position = change.position;

                    if (_lookup.TryGetValue(position, out int index))
                    {
                        if (change.tile != null && _modifierMap != null &&
                            _modifierMap.TryGetValue(change.tile, out NavMeshModifierTilemap.TileModifier tileModifier))
                        {
                            NavMeshBuildSource source = _sources[index];
                            source.area = tileModifier.area;
                            _sources[index] = source;
                        }
                        else if (_modifier != null && _modifier.overrideArea)
                        {
                            NavMeshBuildSource source = _sources[index];
                            source.area = _modifier.area;
                            _sources[index] = source;
                        }
                    }
                }
            }
        }

        public AsyncOperation UpdateNavMesh(NavMeshData data)
        {
            if (_sources == null || NavMeshSurfaceOwner == null)
                return null;

            return NavMeshBuilder.UpdateNavMeshDataAsync(data, NavMeshSurfaceOwner.GetBuildSettings(), _sources, data.sourceBounds);
        }

        public AsyncOperation UpdateNavMesh()
        {
            if (NavMeshSurfaceOwner == null || NavMeshSurfaceOwner.navMeshData == null)
                return null;

            return UpdateNavMesh(NavMeshSurfaceOwner.navMeshData);
        }

        public override void PostCollectSources(NavMeshSurface surface, List<NavMeshBuildSource> sources, NavMeshBuilderState navNeshState)
        {
            _sources = sources;

            if (_tilemap == null)
                return;

            if (_lookup == null)
            {
                _lookup = new Dictionary<Vector3Int, int>();
                for (int i = 0; i < _sources.Count; i++)
                {
                    NavMeshBuildSource source = _sources[i];
                    Vector3Int position = _tilemap.WorldToCell(source.transform.GetPosition());
                    _lookup[position] = i;
                }
            }
        }

        protected override void OnDestroy()
        {
            Tilemap.tilemapTileChanged -= OnTilemapTileChangedModern;
            base.OnDestroy();
        }
    }
}*/