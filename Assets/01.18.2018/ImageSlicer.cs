using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSlicer : MonoBehaviour {

    public static Texture2D[,] GetSlices(Texture2D image, int blockPerLine)
    {
        int imageSize = Mathf.Min(image.width, image.height);

        int blockSize = imageSize / blockPerLine;

        Texture2D[,] blocks = new Texture2D[blockPerLine, blockPerLine];

        for (int Y = 0; Y < blockPerLine; Y++)
        {
            for (int X = 0; X < blockPerLine; X++)
            {
                Texture2D block = new Texture2D(blockSize, blockSize);

                block.wrapMode = TextureWrapMode.Repeat;

                block.SetPixels(image.GetPixels(X*blockSize, Y*blockSize, blockSize, blockSize));
                block.Apply();

                blocks[X, Y] = block;
            }
        }
        return blocks;
    }
}
