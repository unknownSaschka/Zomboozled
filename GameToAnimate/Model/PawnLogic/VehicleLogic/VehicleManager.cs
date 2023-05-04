using Model.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Model.PawnLogic.VehicleLogic
{
    public enum CarType { Car, Tank, Harvester }
    public class VehicleManager
    {
        private MultiValueDictionary<Vector2, Vehicle> _vehicles = new MultiValueDictionary<Vector2, Vehicle>();
        private Vehicle activeVehicle;

        public Vehicle ActiveVehicle { get { return activeVehicle; } set { activeVehicle = value; } }
        
        private IManagerHolder _managerHolder;
        private MainModel _model;
        
        public VehicleManager(IManagerHolder managerHolder, MainModel model)
        {
            _managerHolder = managerHolder;
            _model = model;
        }

        public void AddCar(Vector2 position, Vector2 chunk, float radius, float speed, int lifePoints, CarType carType)
        {
            _vehicles.Add(chunk, new Vehicle(position, chunk, radius, speed, lifePoints, carType));
        }

        public void RemoveCar(Vehicle vehicle)
        {
            if(_vehicles.Contains(vehicle.Chunk, vehicle))
            {
                Console.WriteLine("Removed es auch");
                _vehicles.Remove(vehicle.Chunk, vehicle);
            }
        }

        public void Update()
        {
            if(activeVehicle != null)
            {
                //Console.WriteLine(activeVehicle.Position);
                
                if (activeVehicle.UpdateCurrentChunk())
                {
                    _managerHolder.PlayerManager.Player.MoveToPosition(activeVehicle.Position, activeVehicle.Chunk);
                    _managerHolder.PlayerManager.PlayerChunkChangedEvent?.Invoke();

                    //_vehicles.Remove(activeVehicle.Chunk, activeVehicle);

                    //_vehicles.Add(activeVehicle.Chunk, activeVehicle);
                }
            }
        }

        public IEnumerable<Vehicle> GetNearbyVehicles(Vector2 chunkPos, int interactionDistance)
        {
            return _vehicles.Where(i => i.Key.X <= chunkPos.X + interactionDistance && i.Key.X >= chunkPos.X - interactionDistance && i.Key.Y <= chunkPos.Y + interactionDistance && i.Key.Y >= chunkPos.Y - interactionDistance).SelectMany(i => i.Value);
        }

        public IEnumerable<Vehicle> GetVehicleOfChunk(Vector2 chunk)
        {
            return _vehicles.Where(i => i.Key == chunk).SelectMany(i => i.Value);
        }

        public void RemoveVehicles()
        {
            _vehicles.Clear();
        }

        public void InteractWithCar(bool delete)
        {
            var playerManager = _managerHolder.PlayerManager;
            Vehicle temp = null;
            if (!playerManager.Player.IsInCar)
            {
                foreach (Vehicle vehicle in _managerHolder.VehicleManager.GetNearbyVehicles(playerManager.Player.Chunk, 1))
                {
                    if (Vector2.Distance(playerManager.Player.Position + playerManager.Player.Chunk * 5, vehicle.Position + vehicle.Chunk * 5) < 0.8f)
                    {
                        _model.MovementLogic = new MovementLogicCarPhysics(vehicle, _managerHolder.PlayerManager);
                        playerManager.Player.IsInCar = true;
                        _managerHolder.VehicleManager.ActiveVehicle = vehicle;
                        temp = vehicle;
                        //AudioEvent?.Invoke(AudioType.BGMusic, (int)AudioManager.BackgroundSongName.Teriyaky);
                        _managerHolder.AudioManager.PlaySong(BackgroundSongName.Teriyaky);
                        break;
                    }
                }
                if (temp != null)
                {
                    RemoveCar(temp);
                }
            }
            else
            {
                _model.MovementLogic = new MovementLogicFoot(playerManager.Player);
                playerManager.Player.IsInCar = false;
                if (_managerHolder.VehicleManager.ActiveVehicle != null)
                {
                    if (delete)
                    {
                        Console.WriteLine("Remove");
                        RemoveCar(activeVehicle);
                        activeVehicle = null;
                        //RemoveCar(activeVehicle);
                    }
                    else
                    {
                        Console.WriteLine("Nochmal Add");
                        _vehicles.Add(_managerHolder.VehicleManager.ActiveVehicle.Chunk, _managerHolder.VehicleManager.ActiveVehicle);
                    }

                }

                _managerHolder.VehicleManager.ActiveVehicle = null;
                if (_model.WorldManager.ActiveWorldNum != 0)
                {
                    _managerHolder.AudioManager.ChangeBackgroundMusic((_managerHolder.TileManager.tiles[_model.World.chunks[playerManager.Player.Chunk].TileId].chunkType), (int)playerManager.Distance);
                }
            }
        }
    }
}
