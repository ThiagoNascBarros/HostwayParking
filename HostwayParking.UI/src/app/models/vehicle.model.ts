export interface Vehicle {
  plate: string;
  model: string;
  color: string;
  type: VehicleType;
}

export type VehicleType = 'Motorcycle' | 'Car';

export interface RegisterVehicleRequest {
  plate: string;
  model: string;
  color: string;
  type: VehicleType;
}

export interface UpdateVehicleRequest {
  model?: string;
  color?: string;
  type?: VehicleType;
}
