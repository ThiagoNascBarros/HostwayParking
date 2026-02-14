import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { Parking } from '../../models/parking.model';

@Component({
  selector: 'app-parking-list',
  imports: [CommonModule],
  template: `
    <div class="container">
      <h2>Estacionamentos Cadastrados</h2>
      
      @if (loading()) {
        <p>Carregando...</p>
      } @else if (error()) {
        <p class="error">{{ error() }}</p>
      } @else if (parkings().length === 0) {
        <p>Nenhum estacionamento cadastrado.</p>
      } @else {
        <div class="parking-grid">
          @for (parking of parkings(); track parking.id) {
            <div class="parking-card">
              <h3>{{ parking.name }}</h3>
              <p><strong>Endereço:</strong> {{ parking.address }}</p>
            </div>
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
    }

    h2 {
      color: #333;
      margin-bottom: 20px;
    }

    .parking-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 20px;
    }

    .parking-card {
      background: white;
      border: 1px solid #ddd;
      border-radius: 8px;
      padding: 20px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .parking-card h3 {
      margin: 0 0 10px 0;
      color: #2196F3;
    }

    .parking-card p {
      margin: 5px 0;
    }

    .slots {
      margin-top: 15px;
      padding-top: 15px;
      border-top: 1px solid #eee;
    }

    .slots div {
      margin: 5px 0;
    }

    .price {
      color: #4CAF50;
      margin-left: 10px;
      font-weight: bold;
    }

    .error {
      color: #f44336;
      padding: 10px;
      background: #ffebee;
      border-radius: 4px;
    }
  `]
})
export class ParkingListComponent {
  private readonly apiService = inject(ApiService);
  
  parkings = signal<Parking[]>([]);
  loading = signal(true);
  error = signal('');

  constructor() {
    this.loadParkings();
  }

  loadParkings(): void {
    this.loading.set(true);
    this.apiService.getAllParkings().subscribe({
      next: (parkings: any[]) => {
        console.log('Parkings received:', parkings);
        console.log('First parking object:', JSON.stringify(parkings[0], null, 2));
        const normalized = parkings.map(p => ({
          id: p.id || p.Id || p.code || p.Code,
          name: p.name || p.Name || p.city || p.City || 'Sem nome',
          address: p.address || p.Address || p.city || p.City || 'Sem endereço',
          motorcycleSlots: p.motorcycleSlots ?? p.MotorcycleSlots ?? p.motorcycleSpots ?? p.MotorcycleSpots ?? 0,
          carSlots: p.carSlots ?? p.CarSlots ?? p.carSpots ?? p.CarSpots ?? 0,
          pricePerHourMotorcycle: p.pricePerHourMotorcycle ?? p.PricePerHourMotorcycle ?? p.motorcyclePrice ?? p.MotorcyclePrice ?? 0,
          pricePerHourCar: p.pricePerHourCar ?? p.PricePerHourCar ?? p.carPrice ?? p.CarPrice ?? 0
        }));
        console.log('Normalized parkings:', normalized);
        this.parkings.set(normalized as any);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Erro ao carregar estacionamentos: ' + err.message);
        this.loading.set(false);
      }
    });
  }
}
