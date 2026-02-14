import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { RegisterParkingRequest } from '../../models/parking.model';

@Component({
  selector: 'app-parking-register',
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <h2>Cadastrar Estacionamento</h2>
      
      <form (ngSubmit)="onSubmit()" #form="ngForm">
        <div class="form-group">
          <label for="name">Nome:</label>
          <input 
            type="text" 
            id="name" 
            name="name"
            [(ngModel)]="parking.name" 
            required 
            #name="ngModel"
          />
          @if (name.invalid && name.touched) {
            <span class="error">Nome é obrigatório</span>
          }
        </div>

        <div class="form-group">
          <label for="address">Endereço:</label>
          <input 
            type="text" 
            id="address" 
            name="address"
            [(ngModel)]="parking.address" 
            required 
            #address="ngModel"
          />
          @if (address.invalid && address.touched) {
            <span class="error">Endereço é obrigatório</span>
          }
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="motorcycleSlots">Vagas para Motos:</label>
            <input 
              type="number" 
              id="motorcycleSlots" 
              name="motorcycleSlots"
              [(ngModel)]="parking.motorcycleSlots" 
              required 
              min="0"
              #motorcycleSlots="ngModel"
            />
          </div>

          <div class="form-group">
            <label for="pricePerHourMotorcycle">Preço/Hora Moto (R$):</label>
            <input 
              type="number" 
              id="pricePerHourMotorcycle" 
              name="pricePerHourMotorcycle"
              [(ngModel)]="parking.pricePerHourMotorcycle" 
              required 
              min="0"
              step="0.01"
              #pricePerHourMotorcycle="ngModel"
            />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="carSlots">Vagas para Carros:</label>
            <input 
              type="number" 
              id="carSlots" 
              name="carSlots"
              [(ngModel)]="parking.carSlots" 
              required 
              min="0"
              #carSlots="ngModel"
            />
          </div>

          <div class="form-group">
            <label for="pricePerHourCar">Preço/Hora Carro (R$):</label>
            <input 
              type="number" 
              id="pricePerHourCar" 
              name="pricePerHourCar"
              [(ngModel)]="parking.pricePerHourCar" 
              required 
              min="0"
              step="0.01"
              #pricePerHourCar="ngModel"
            />
          </div>
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
      max-width: 600px;
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

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 15px;
    }

    label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
      color: #555;
    }

    input {
      width: 100%;
      padding: 8px 12px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
    }

    input:focus {
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
export class ParkingRegisterComponent {
  private readonly apiService = inject(ApiService);
  private readonly router = inject(Router);

  parking: RegisterParkingRequest = {
    name: '',
    address: '',
    motorcycleSlots: 0,
    carSlots: 0,
    pricePerHourMotorcycle: 0,
    pricePerHourCar: 0
  };

  loading = signal(false);
  error = signal('');
  success = signal('');

  onSubmit(): void {
    this.loading.set(true);
    this.error.set('');
    this.success.set('');

    this.apiService.registerParking(this.parking).subscribe({
      next: () => {
        this.success.set('Estacionamento cadastrado com sucesso!');
        this.loading.set(false);
        setTimeout(() => {
          this.router.navigate(['/parkings']);
        }, 1500);
      },
      error: (err) => {
        this.error.set('Erro ao cadastrar estacionamento: ' + err.message);
        this.loading.set(false);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/parkings']);
  }
}
