import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ParkingListComponent } from './components/parking-list/parking-list.component';
import { ParkingRegisterComponent } from './components/parking-register/parking-register.component';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { VehicleCreateComponent } from './components/vehicle-create/vehicle-create.component';
import { SessionManagementComponent } from './components/session-management/session-management.component';
import { ReportsComponent } from './components/reports/reports.component';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'parkings', component: ParkingListComponent },
  { path: 'parkings/new', component: ParkingRegisterComponent },
  { path: 'vehicles', component: VehicleListComponent },
  { path: 'vehicles/new', component: VehicleCreateComponent },
  { path: 'sessions', component: SessionManagementComponent },
  { path: 'reports', component: ReportsComponent },
  { path: '**', redirectTo: '' }
];
