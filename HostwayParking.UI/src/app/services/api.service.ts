import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Parking, RegisterParkingRequest } from '../models/parking.model';
import { 
  Vehicle, 
  RegisterVehicleRequest, 
  UpdateVehicleRequest 
} from '../models/vehicle.model';
import { 
  Session, 
  CheckInRequest, 
  CheckOutRequest,
  CheckOutPreview,
  CheckOutResult
} from '../models/session.model';
import {
  RevenueByDay,
  TopVehicle,
  OccupancyByHour
} from '../models/report.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'https://localhost:7062/api';

  // Parking endpoints
  registerParking(parking: RegisterParkingRequest): Observable<Parking> {
    return this.http.post<Parking>(`${this.baseUrl}/Parking`, parking);
  }

  getAllParkings(): Observable<Parking[]> {
    return this.http.get<Parking[]>(`${this.baseUrl}/Parking`);
  }

  // Vehicle endpoints
  createVehicle(vehicle: RegisterVehicleRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/Vehicle`, vehicle);
  }

  updateVehicle(plate: string, vehicle: UpdateVehicleRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Vehicle/${plate}`, vehicle);
  }

  getAllVehicles(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.baseUrl}/Vehicle`);
  }

  // Session endpoints
  checkIn(request: CheckInRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/SessionParking/checkin`, request);
  }

  checkOut(request: CheckOutRequest): Observable<CheckOutResult> {
    return this.http.post<CheckOutResult>(`${this.baseUrl}/SessionParking/checkout`, request);
  }

  getActiveSessions(): Observable<Session[]> {
    return this.http.get<Session[]>(`${this.baseUrl}/SessionParking`);
  }

  getCheckOutPreview(plate: string): Observable<CheckOutPreview> {
    return this.http.get<CheckOutPreview>(`${this.baseUrl}/SessionParking/checkout/preview/${plate}`);
  }

  // Report endpoints
  getRevenueByDay(days: number): Observable<RevenueByDay[]> {
    return this.http.get<RevenueByDay[]>(`${this.baseUrl}/Report/revenue?days=${days}`);
  }

  getTopVehiclesByTime(start: Date, end: Date): Observable<TopVehicle[]> {
    return this.http.get<TopVehicle[]>(
      `${this.baseUrl}/Report/top-vehicles?start=${start.toISOString()}&end=${end.toISOString()}`
    );
  }

  getOccupancyByHour(start: Date, end: Date): Observable<OccupancyByHour[]> {
    return this.http.get<OccupancyByHour[]>(
      `${this.baseUrl}/Report/occupancy?start=${start.toISOString()}&end=${end.toISOString()}`
    );
  }
}
