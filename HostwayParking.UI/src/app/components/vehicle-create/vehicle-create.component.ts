import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { RegisterVehicleRequest, VehicleType } from '../../models/vehicle.model';

@Component({
  selector: 'app-vehicle-create',
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <h2>Cadastrar Veículo</h2>
      
      <form (ngSubmit)="onSubmit()" #form="ngForm">
        <div class="form-group">
          <label for="plate">Placa:</label>
          <input 
            type="text" 
            id="plate" 
            name="plate"
            [(ngModel)]="vehicle.plate" 
            required 
            #plate="ngModel"
            placeholder="ABC-1234"
          />
          @if (plate.invalid && plate.touched) {
            <span class="error">Placa é obrigatória</span>
          }
        </div>

        <div class="form-group">
          <label for="model">Modelo:</label>
          <input 
            type="text" 
            id="model" 
            name="model"
            [(ngModel)]="vehicle.model" 
            required 
            #model="ngModel"
            placeholder="Honda Civic"
          />
          @if (model.invalid && model.touched) {
            <span class="error">Modelo é obrigatório</span>
          }
        </div>

        <div class="form-group">
          <label for="color">Cor:</label>
          <input 
            type="text" 
            id="color" 
            name="color"
            [(ngModel)]="vehicle.color" 
            required 
            #color="ngModel"
            placeholder="Preto"
          />
          @if (color.invalid && color.touched) {
            <span class="error">Cor é obrigatória</span>
          }
        </div>

        <div class="form-group">
          <label for="type">Tipo:</label>
          <select 
            id="type" 
            name="type"
            [(ngModel)]="vehicle.type" 
            required 
            #type="ngModel"
          >
            <option value="">Selecione...</option>
            <option value="Motorcycle">Moto</option>
            <option value="Car">Carro</option>
          </select>
          @if (type.invalid && type.touched) {
            <span class="error">Tipo é obrigatório</span>
          }
        </div>

        @if (error()) {
          <p class="error">{{ error() }}</p>
        }

        @if (success()) {
          <p class="success">{{ success() }}</p>
        }

        <div class="form-actions">
          <button type="submit" [disabled]="form.invalid || loading()">
            {{ loading() ? 'Cadastrando...' : 'Cadastrar' }}
          </button>
          <button type="button" (click)="onCancel()">Cancelar</button>
        </div>
      </form>
    </div>
  `,
  styles: [`
    .container {
      max-width: 500px;
      margin: 0 auto;
      padding: 20px;
    }

    h2 {
      color: #333;
      margin-bottom: 20px;
    }

    .form-group {
      margin-bottom: 15px;
    }

    label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
      color: #555;
    }

    input,
    select {
      width: 100%;
      padding: 8px 12px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
    }

    input:focus,
    select:focus {
      outline: none;
      border-color: #2196F3;
    }

    .form-actions {
      display: flex;
      gap: 10px;
      margin-top: 20px;
    }

    button {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      font-weight: 500;
    }

    button[type="submit"] {
      background: #2196F3;
      color: white;
    }

    button[type="submit"]:hover:not(:disabled) {
      background: #1976D2;
    }

    button[type="submit"]:disabled {
      background: #ccc;
      cursor: not-allowed;
    }

    button[type="button"] {
      background: #f5f5f5;
      color: #333;
    }

    button[type="button"]:hover {
      background: #e0e0e0;
    }

    .error {
      color: #f44336;
      font-size: 12px;
      margin-top: 5px;
    }

    .success {
      color: #4CAF50;
      padding: 10px;
      background: #e8f5e9;
      border-radius: 4px;
      margin-bottom: 15px;
    }
  `]
})
export class VehicleCreateComponent {
  private readonly apiService = inject(ApiService);
  private readonly router = inject(Router);

  vehicle: RegisterVehicleRequest = {
    plate: '',
    model: '',
    color: '',
    type: '' as VehicleType
  };

  loading = signal(false);
  error = signal('');
  success = signal('');

  onSubmit(): void {
    this.loading.set(true);
    this.error.set('');
    this.success.set('');

    this.apiService.createVehicle(this.vehicle).subscribe({
      next: () => {
        this.success.set('Veículo cadastrado com sucesso!');
        this.loading.set(false);
        setTimeout(() => {
          this.router.navigate(['/vehicles']);
        }, 1500);
      },
      error: (err) => {
        this.error.set('Erro ao cadastrar veículo');
        this.loading.set(false);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/vehicles']);
  }
}
