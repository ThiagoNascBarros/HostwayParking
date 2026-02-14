export interface Parking {
  id: string;
  name: string;
  address: string;
  motorcycleSlots: number;
  carSlots: number;
  pricePerHourMotorcycle: number;
  pricePerHourCar: number;
}

export interface RegisterParkingRequest {
  name: string;
  address: string;
  motorcycleSlots: number;
  carSlots: number;
  pricePerHourMotorcycle: number;
  pricePerHourCar: number;
}
