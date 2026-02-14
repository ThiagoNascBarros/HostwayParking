import { VehicleType } from './vehicle.model';

export interface Session {
  id?: string;
  plate: string;
  model: string;
  color: string;
  type?: VehicleType;
  entryTime: string;
  parkingId?: string;
}

export interface CheckInRequest {
  plate: string;
  model: string;
  color: string;
  type: VehicleType;
}

export interface CheckOutRequest {
  plate: string;
}

export interface CheckOutPreview {
  plate: string;
  model: string;
  entryTime: string;
  exitTime: string;
  timeSpent: string;
  totalPrice: number;
  currency: string;
}

export interface CheckOutResult {
  plate: string;
  model: string;
  entryTime: string;
  exitTime: string;
  timeSpent: string;
  totalPrice: number;
  currency: string;
}
