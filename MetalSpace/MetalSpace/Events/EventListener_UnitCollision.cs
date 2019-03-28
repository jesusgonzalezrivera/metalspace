using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_UnitCollision</c> class represents the behaviour
    /// when a <c>MoveableObject</c> collision with the map.
    /// 
    /// Respond to: EventData_ObjectsCollision, EventData_PlaneCollision
    /// </summary>
    class EventListener_UnitCollision : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_UnitCollision</c> class.
        /// </summary>
        public EventListener_UnitCollision()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Unit_Collision_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when a <c>MoveableObject</c> collide with
        /// the map.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (!(producedEvent is EventData_ObjectsCollision) && !(producedEvent is EventData_PlaneCollision))
                return false;

            if (producedEvent is EventData_PlaneCollision)
            {
                Vector3 point1, point2, point3, point4;
                float numerator, denominator, distance1, distance2, distance3, distance4;
                Plane plane = ((EventData_PlaneCollision)producedEvent).BoundingPlane;
                BoundingSphere sphere = ((EventData_PlaneCollision)producedEvent).MObject.DModel.BSphere;

                point1 = new Vector3(sphere.Center.X + sphere.Radius, sphere.Center.Y, sphere.Center.Z);
                numerator = Math.Abs(
                    plane.Normal.X * point1.X + plane.Normal.Y * point1.Y + plane.Normal.Z * point1.Z + plane.D);
                denominator = (float)Math.Sqrt(
                    plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z);
                distance1 = numerator / denominator;

                point2 = new Vector3(sphere.Center.X - sphere.Radius, sphere.Center.Y, sphere.Center.Z);
                numerator = Math.Abs(
                    plane.Normal.X * point2.X + plane.Normal.Y * point2.Y + plane.Normal.Z * point2.Z + plane.D);
                denominator = (float)Math.Sqrt(
                    plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z);
                distance2 = numerator / denominator;

                point3 = new Vector3(sphere.Center.X, sphere.Center.Y + sphere.Radius, sphere.Center.Z);
                numerator = Math.Abs(
                    plane.Normal.X * point3.X + plane.Normal.Y * point3.Y + plane.Normal.Z * point3.Z + plane.D);
                denominator = (float)Math.Sqrt(
                    plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z);
                distance3 = numerator / denominator;

                point4 = new Vector3(sphere.Center.X, sphere.Center.Y - sphere.Radius, sphere.Center.Z);
                numerator = Math.Abs(
                    plane.Normal.X * point4.X + plane.Normal.Y * point4.Y + plane.Normal.Z * point4.Z + plane.D);
                denominator = (float)Math.Sqrt(
                    plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z);
                distance4 = numerator / denominator;

                if (distance1 < distance2 && distance1 < distance3 && distance1 < distance4)
                {
                    // Right collision
                    ((EventData_PlaneCollision)producedEvent).MObject.CPosition = new Vector3(
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.X - (Math.Abs(distance1) < 0.1f ? 0 : distance1),
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Y,
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Z);
                }
                else if (distance2 < distance1 && distance2 < distance3 && distance2 < distance4)
                {
                    // Left collision
                    ((EventData_PlaneCollision)producedEvent).MObject.CPosition = new Vector3(
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.X + (Math.Abs(distance2) < 0.1f ? 0 : distance2),
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Y,
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Z);
                }
                else if (distance3 < distance1 && distance3 < distance2 && distance3 < distance4)
                {
                    // Up collision
                    ((EventData_PlaneCollision)producedEvent).MObject.CPosition = new Vector3(
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.X,
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Y - (Math.Abs(distance3) < 0.1f ? 0 : distance3),
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Z);
                }
                else
                {
                    // Down collision
                    ((EventData_PlaneCollision)producedEvent).MObject.CPosition = new Vector3(
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.X,
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Y + (Math.Abs(distance4) < 0.1f ? 0 : distance4),
                        ((EventData_PlaneCollision)producedEvent).MObject.CPosition.Z);
                }

                return false;
            }
            else
            {
                switch (((EventData_ObjectsCollision)producedEvent).Direction)
                {
                    // Up collision
                    case EventData_ObjectsCollision.CollisionDirection.Up:
                        ((EventData_ObjectsCollision)producedEvent).MObject.CPosition = new Vector3(
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.X,
                            ((EventData_ObjectsCollision)producedEvent).Node.BoundingBox.Min.Y + ((EventData_ObjectsCollision)producedEvent).AddValue,
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Z);
                        break;

                    // Down collision
                    case EventData_ObjectsCollision.CollisionDirection.Down:
                        if (((EventData_ObjectsCollision)producedEvent).Type == EventData_ObjectsCollision.CollisionSurface.Box)
                        {
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition = new Vector3(
                                ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.X,
                                ((EventData_ObjectsCollision)producedEvent).Node.BoundingBox.Max.Y + ((EventData_ObjectsCollision)producedEvent).AddValue,
                                ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Z);
                        }
                        else if (((EventData_ObjectsCollision)producedEvent).Type == EventData_ObjectsCollision.CollisionSurface.Plane)
                        {
                            Vector3 L1 = ((EventData_ObjectsCollision)producedEvent).MObject.DModel.BSphere.Center;
                            Vector3 L2 = new Vector3(L1.X, L1.Y - ((EventData_ObjectsCollision)producedEvent).MObject.DModel.BSphere.Radius, L1.Z);
                            Vector3 LV = L2 - L1;
                            Ray ray = new Ray(L1, LV);

                            float distance = (float)(ray.Intersects(((EventData_ObjectsCollision)producedEvent).Node.BoundingPlane) == null ? 0f :
                                ray.Intersects(((EventData_ObjectsCollision)producedEvent).Node.BoundingPlane));
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition = new Vector3(
                                ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.X,
                                L1.Y + distance * LV.Y + ((EventData_ObjectsCollision)producedEvent).AddValue,
                                ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Z);
                        }

                        break;

                    // Left collision
                    case EventData_ObjectsCollision.CollisionDirection.Left:
                        ((EventData_ObjectsCollision)producedEvent).MObject.CPosition = new Vector3(
                            ((EventData_ObjectsCollision)producedEvent).Node.BoundingBox.Max.X + ((EventData_ObjectsCollision)producedEvent).AddValue,
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Y,
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Z);

                        break;
                    
                    // Right collision
                    case EventData_ObjectsCollision.CollisionDirection.Right:
                        ((EventData_ObjectsCollision)producedEvent).MObject.CPosition = new Vector3(
                            ((EventData_ObjectsCollision)producedEvent).Node.BoundingBox.Min.X + ((EventData_ObjectsCollision)producedEvent).AddValue,
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Y,
                            ((EventData_ObjectsCollision)producedEvent).MObject.CPosition.Z);

                        break;
                }

                return true;
            }
        }

    }
}
