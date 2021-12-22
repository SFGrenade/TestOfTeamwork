using tk2dRuntime.TileMap;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    public class PatchTileMap : MonoBehaviour
    {
        public int width;
        public int height;
        public int columns;
        public int rows;
        public int partSizeX;
        public int partSizeY;
        public PhysicsMaterial2D physicsMaterial2D;
        public GameObject renderData;

        private void Awake()
        {
            var tileMap = gameObject.AddComponent<tk2dTileMap>();
            tileMap.renderData = renderData;
            tileMap.width = width;
            tileMap.height = height;
            tileMap.partitionSizeX = partSizeX;
            tileMap.partitionSizeY = partSizeY;
            tileMap.Layers = new Layer[]
            {
                new Layer(0, width, height, partSizeX, partSizeY)
                {
                    gameObject = renderData.transform.GetChild(0).gameObject,
                    numColumns = columns,
                    numRows = rows
                }
            };
            tileMap.ColorChannel = new ColorChannel(width, height, partSizeX, partSizeY)
            {
                clearColor = new Color(1, 1, 1, 1),
                numColumns = columns,
                numRows = rows
            };
        }
    }
}
