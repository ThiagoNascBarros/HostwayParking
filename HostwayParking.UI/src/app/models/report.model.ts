export interface RevenueByDay {
  date: string;
  revenue: number;
}

export interface TopVehicle {
  plate: string;
  model: string;
  totalTime: string;
  visits: number;
  totalMinutes?: number;
}

export interface OccupancyByHour {
  hour: number;
  hourRange: string;
  averageVehicles: number;
  maxVehicles: number;
  occupancy?: number;
}
