import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Vehicle } from '../../models/vehicle.model';

@Component({
  selector: 'app-vehicle-list',
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="header">
        <h2>Veículos Cadastrados</h2>
        <button (click)="onAddVehicle()">+ Adicionar Veículo</button>
      </div>
      
      @if (loading()) {
        <p>Carregando...</p>
      } @else if (error()) {
        <p class="error">{{ error() }}</p>
      } @else if (vehicles().length === 0) {
        <p>Nenhum veículo cadastrado.</p>
      } @else {
        <table>
          <thead>
            <tr>
              <th>Placa</th>
              <th>Modelo</th>
              <th>Cor</th>
              <th>Tipo</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            @for (vehicle of vehicles(); track vehicle.plate) {
              <tr>
                <td>{{ vehicle.plate }}</td>
                <td>{{ vehicle.model }}</td>
                <td>{{ vehicle.color }}</td>
                <td>
                  <span [class]="'type-badge ' + (vehicle.type ? vehicle.type.toLowerCase() : '')">
                    {{ vehicle.type === 'Motorcycle' ? 'Moto' : 'Carro' }}
                  </span>
                </td>
                <td>
                  <button class="btn-edit" (click)="onEdit(vehicle)">Editar</button>
                </td>
              </tr>
            }
          </tbody>
        </table>
      }

      @if (editingVehicle()) {
        <div class="modal">
          <div class="modal-content">
            <h3>Editar Veículo - {{ editingVehicle()!.plate }}</h3>
            <form (ngSubmit)="onUpdate()">
              <div class="form-group">
                <label>Modelo:</label>
                <input type="text" [(ngModel)]="editForm.model" name="model" required />
              </div>
              <div class="form-group">
                <label>Cor:</label>
                <input type="text" [(ngModel)]="editForm.color" name="color" required />
              </div>
              <div class="form-group">
                <label>Tipo:</label>
                <select [(ngModel)]="editForm.type" name="type" required>
                  <option value="Motorcycle">Moto</option>
                  <option value="Car">Carro</option>
                </select>
              </div>
              <div class="form-actions">
                <button type="submit">Salvar</button>
                <button type="button" (click)="editingVehicle.set(null)">Cancelar</button>
              </div>
            </form>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }

    h2 {
      color: #333;
      margin: 0;
    }

    button {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      font-weight: 500;
      background: #2196F3;
      color: white;
    }

    button:hover {
      background: #1976D2;
    }

    table {
      width: 100%;
      background: white;
      border-collapse: collapse;
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    thead {
      background: #2196F3;
      color: white;
    }

    th, td {
      padding: 15px;
      text-align: left;
    }

    tbody tr:nth-child(even) {
      background: #f5f5f5;
    }

    tbody tr:hover {
      background: #e3f2fd;
    }

    .type-badge {
      padding: 4px 12px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
    }

    .type-badge.motorcycle {
      background: #e1bee7;
      color: #7b1fa2;
    }

    .type-badge.car {
      background: #bbdefb;
      color: #1976d2;
    }

    .btn-edit {
      padding: 6px 12px;
      font-size: 12px;
      background: #4CAF50;
    }

    .btn-edit:hover {
      background: #45a049;
    }

    .modal {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: rgba(0,0,0,0.5);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 1000;
    }

    .modal-content {
      background: white;
      padding: 30px;
      border-radius: 8px;
      max-width: 400px;
      width: 90%;
    }

    .form-group {
      margin-bottom: 15px;
    }

    .form-group label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
    }

    .form-group input,
    .form-group select {
      width: 100%;
      padding: 8px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }

    .form-actions {
      display: flex;
      gap: 10px;
      margin-top: 20px;
    }

    .form-actions button[type="button"] {
      background: #f5f5f5;
      color: #333;
    }

    .error {
      color: #f44336;
      padding: 10px;
      background: #ffebee;
      border-radius: 4px;
    }
  `]
})
export class VehicleListComponent {
  private readonly apiService = inject(ApiService);
  private readonly router = inject(Router);
  
  vehicles = signal<Vehicle[]>([]);
  loading = signal(true);
  error = signal('');
  editingVehicle = signal<Vehicle | null>(null);
  editForm: any = {};

  constructor() {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.loading.set(true);
    this.apiService.getAllVehicles().subscribe({
      next: (vehicles: any[]) => {
        const normalized = vehicles.map(v => ({
          plate: v.plate || v.Plate,
          model: v.model || v.Model,
          color: v.color || v.Color,
          type: v.type || v.Type
        }));
        this.vehicles.set(normalized as any);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Erro ao carregar veículos: ' + err.message);
        this.loading.set(false);
      }
    });
  }

  onAddVehicle(): void {
    this.router.navigate(['/vehicles/new']);
  }

  onEdit(vehicle: Vehicle): void {
    this.editingVehicle.set(vehicle);
    this.editForm = {
      model: vehicle.model,
      color: vehicle.color,
      type: vehicle.type
    };
  }

  onUpdate(): void {
    const vehicle = this.editingVehicle();
    if (!vehicle) return;

    this.apiService.updateVehicle(vehicle.plate, this.editForm).subscribe({
      next: () => {
        this.editingVehicle.set(null);
        this.loadVehicles();
      },
      error: (err) => {
        this.error.set('Erro ao atualizar veículo: ' + err.message);
      }
    });
  }
}
