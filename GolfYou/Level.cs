﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace GolfYou
{
    public class Level
    {
        private TiledMap map;
        private Dictionary<int, TiledTileset> tilesets;
        private Texture2D tilesetTexture;
        private Texture2D tilesetTexture2;
        private TiledLayer collisionLayer;
        private TiledLayer endLayer;

        [Flags]
        enum Trans
        {
            None = 0,
            Flip_H = 1 << 0,
            Flip_V = 1 << 1,
            Flip_D = 1 << 2,

            Rotate_90 = Flip_D | Flip_H,
            Rotate_180 = Flip_H | Flip_V,
            Rotate_270 = Flip_V | Flip_D,

            Rotate_90AndFlip_H = Flip_H | Flip_V | Flip_D,
        }

        public void loadLevel(ContentManager Content, string levelName)
        {
            map = new TiledMap(Content.RootDirectory + "/Levels/" + levelName);
            tilesets = map.GetTiledTilesets(Content.RootDirectory + "/Levels/");

            tilesetTexture = Content.Load<Texture2D>("Levels/LevelMaterials/Terrain (32x32)");
            tilesetTexture2 = Content.Load<Texture2D>("Levels/LevelMaterials/Decorations (32x32)");

            collisionLayer = map.Layers.First(l => l.name == "Collision");
            endLayer = map.Layers.First(l => l.name == "StartEnd");

        }
        
        public void drawLevel(SpriteBatch _spriteBatch)
        {
            var tileLayers = map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

            foreach (var layer in tileLayers)
            {
                for (var y = 0; y < layer.height; y++)
                {
                    for (var x = 0; x < layer.width; x++)
                    {
                        var index = (y * layer.width) + x; // Assuming the default render order is used which is from right to bottom
                        var gid = layer.data[index]; // The tileset tile index
                        var tileX = x * map.TileWidth;
                        var tileY = y * map.TileHeight;

                        // Gid 0 is used to tell there is no tile set
                        if (gid == 0)
                        {
                            continue;
                        }

                        // Helper method to fetch the right TieldMapTileset instance
                        // This is a connection object Tiled uses for linking the correct tileset to the gid value using the firstgid property
                        var mapTileset = map.GetTiledMapTileset(gid);

                        // Retrieve the actual tileset based on the firstgid property of the connection object we retrieved just now
                        var tileset = tilesets[mapTileset.firstgid];

                        // Use the connection object as well as the tileset to figure out the source rectangle
                        var rect = map.GetSourceRect(mapTileset, tileset, gid);

                        // Create destination and source rectangles
                        var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                        var destination = new Rectangle(tileX, tileY, map.TileWidth, map.TileHeight);


                        // You can use the helper methods to get information to handle flips and rotations
                        Trans tileTrans = Trans.None;
                        if (map.IsTileFlippedHorizontal(layer, x, y)) tileTrans |= Trans.Flip_H;
                        if (map.IsTileFlippedVertical(layer, x, y)) tileTrans |= Trans.Flip_V;
                        if (map.IsTileFlippedDiagonal(layer, x, y)) tileTrans |= Trans.Flip_D;

                        SpriteEffects effects = SpriteEffects.None;
                        double rotation = 0f;
                        switch (tileTrans)
                        {
                            case Trans.Flip_H: effects = SpriteEffects.FlipHorizontally; break;
                            case Trans.Flip_V: effects = SpriteEffects.FlipVertically; break;

                            case Trans.Rotate_90:
                                rotation = Math.PI * .5f;
                                destination.X += map.TileWidth;
                                break;

                            case Trans.Rotate_180:
                                rotation = Math.PI;
                                destination.X += map.TileWidth;
                                destination.Y += map.TileHeight;
                                break;

                            case Trans.Rotate_270:
                                rotation = Math.PI * 3 / 2;
                                destination.Y += map.TileHeight;
                                break;

                            case Trans.Rotate_90AndFlip_H:
                                effects = SpriteEffects.FlipHorizontally;
                                rotation = Math.PI * .5f;
                                destination.X += map.TileWidth;
                                break;

                            default:
                                break;
                        }


                        // Render sprite at position tileX, tileY using the rect
                        if (layer.name == "Terrain" || layer.name == "Background") { _spriteBatch.Draw(tilesetTexture, destination, source, Color.White, (float)rotation, Vector2.Zero, effects, 0); }
                        else { _spriteBatch.Draw(tilesetTexture2, destination, source, Color.White, (float)rotation, Vector2.Zero, effects, 0); }

                    }
                }
            }
        }
        /*public Vector2 ApplyTileCollision(GameTime gameTime, Rectangle player)
        {

            foreach (var obj in collisionLayer.objects)
            {
                var objRect = new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height);
                Debug.WriteLine("Player.Bottom: " + player.Bottom);
                
                if (player.Intersects(objRect))
                {
                    Debug.WriteLine("objRect.Top: " + objRect.Top);
                    player.Y = MathHelper.Clamp(player.Y, 0, objRect.Top + -30);

                }
                /*else if (player.Top == objRect.Bottom && playerPosition.X + 120 > objRect.Left && playerPosition.X + 120 < objRect.Right)
                {
                    playerPosition.Y = MathHelper.Clamp(playerPosition.Y, objRect.Bottom - 85, Window.ClientBounds.Height);

                }
                else if (playerPosition.Y >= objRect.Top - 160 && playerPosition.Y <= objRect.Bottom - 80 && playerPosition.X + 120 <= objRect.Left)
                {
                    playerPosition.X = MathHelper.Clamp(playerPosition.X, -120, objRect.Left - 130);

                }
                else if (playerPosition.Y >= objRect.Top - 160 && playerPosition.Y <= objRect.Bottom - 80 && playerPosition.X + 120 >= objRect.Right)
                {
                    playerPosition.X = MathHelper.Clamp(playerPosition.X, objRect.Right - 110, Window.ClientBounds.Width + 120);

                }

                if (boxPosition.Y <= objRect.Top - 32 && boxPosition.X + 50 > objRect.Left - 10 && boxPosition.X + 50 < objRect.Right + 10)
                {
                    boxPosition.Y = MathHelper.Clamp(boxPosition.Y, -160, objRect.Top - 45);
                    if (boxPosition.Y == objRect.Top - 45) { boxYVel = 0; }

                }
                else if (boxPosition.Y >= objRect.Bottom - 32 && boxPosition.X + 50 > objRect.Left && boxPosition.X + 50 < objRect.Right)
                {
                    boxPosition.Y = MathHelper.Clamp(boxPosition.Y, objRect.Bottom - 35, Window.ClientBounds.Height);

                }
                else if (boxPosition.Y >= objRect.Top - 160 && boxPosition.Y <= objRect.Bottom - 80 && boxPosition.X + 120 <= objRect.Left)
                {
                    boxPosition.X = MathHelper.Clamp(boxPosition.X, -120, objRect.Left - 130);

                }
                else if (boxPosition.Y >= objRect.Top - 160 && boxPosition.Y <= objRect.Bottom - 80 && boxPosition.X + 120 >= objRect.Right)
                {
                    boxPosition.X = MathHelper.Clamp(boxPosition.X, objRect.Right - 110, Window.ClientBounds.Width + 120);

                } 

            }
            var endobj = endLayer.objects.First(l => l.name == "EndSquare");
            var endWindow = new Rectangle((int)endobj.x, (int)endobj.y, (int)endobj.width, (int)endobj.height);
            if (player.Y > endWindow.Top && player.Y< endWindow.Bottom && player.X > endWindow.Left && player.X < endWindow.Right)
            {
                Debug.WriteLine("Got to end!");
            } 

            return new Vector2(player.X, player.Y);

        } */

        public TiledLayer getCollisionLayer()
        {
            return collisionLayer;
        }

        public Vector2 getPlayerSpawnLocation()
        {
            var startobj = endLayer.objects.First(l => l.name == "BeginSquare");
            return new Vector2(startobj.x, startobj.y);
        }

        public Vector2 getMapBounds()
        {
            return new Vector2(map.Width * 32, map.Height * 32);
        }

    }

}

