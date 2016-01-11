﻿using System;
using SFML.Graphics;
using SFML.Window;
using SFMLTest.Helpers;

namespace SFMLTest.Entities
{
    public class Player : EntityWithHead, IProjectileOwner
    {
        private const float DashTime = 0.1f;
        private const float DashCooldown = 1f;
        private const float DashMultiplier = 5;
        private readonly PressListener _spaceListener = new PressListener(Keyboard.Key.Space);
        private SwordProjectile _swordProjectile;
        private readonly PressListener _1ButtonListener;

        private float _dashEndTime;

        public Player() : base(10, 150, Color.Green)
        {
            Position = new Vector2(50, 50);
            _1ButtonListener = new PressListener(Keyboard.Key.Num1);
        }

        public override int RenderPriority
        {
            get { return 1000; }
        }

        protected override float InvisibilityTime
        {
            get { return 0.5f; }
        }

        public override void Update(float dTime)
        {
            base.Update(dTime);

            float speed = Speed;

            if (_dashEndTime > Game.ElapsedTime)
            {
                speed *= DashMultiplier;
            }
            if (_spaceListener.IsPressed && _dashEndTime + DashCooldown < Game.ElapsedTime)
            {
                _dashEndTime = Game.ElapsedTime + DashTime;
            }

            Vector2 direction = GetDirection();
            direction.X *= speed*dTime;
            direction.Y *= speed*dTime;

            if (Math.Abs(direction.X) > float.Epsilon && Math.Abs(direction.Y) > float.Epsilon)
            {
                direction = direction/(float) Math.Sqrt(2);
            }

            Position = CollisionHelper.FindAccessablePoint(this, Position + direction);
            
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && _swordProjectile == null)
            {
                _swordProjectile = new SwordProjectile(this, Position.AngleTo(Game.MousePosition), 0.2f);
                Game.Entities.Add(_swordProjectile);
            }

            if (_1ButtonListener.IsPressed)
            {
                FireballProjectile projectile = new FireballProjectile(this, (Game.MousePosition - Position).Normalize() * 75);
                Game.Entities.Add(projectile);
            }
        }

        public bool ValidateProjectileHit(Projectile projectile, Entity target)
        {
            return target is Enemy;
        }

        protected override Vector2 GetDirection()
        {
            Vector2 direction = new Vector2();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D)) direction.X++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A)) direction.X--;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.W)) direction.Y--;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Keyboard.IsKeyPressed(Keyboard.Key.S)) direction.Y++;

            return direction;
        }

        public void ProjectileDied(Projectile projectile)
        {
            if (projectile is SwordProjectile)
            {
                _swordProjectile = null;
            }
        }
    }
}