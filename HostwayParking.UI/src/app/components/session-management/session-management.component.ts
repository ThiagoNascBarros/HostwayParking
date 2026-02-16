import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Session, CheckInRequest } from '../../models/session.model';
import { VehicleType } from '../../models/vehicle.model';

@Component({
  selector: 'app-session-management',
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="section">
        <h2>Check-In de Veículo</h2>
        <form (ngSubmit)="onCheckIn()" #checkInForm="ngForm">
          <div class="form-row">
            <div class="form-group">
              <label>Placa:</label>
              <input type="text" [(ngModel)]="checkInData.plate" name="plate" required placeholder="ABC-1234" />
            </div>
            <div class="form-group">
              <label>Modelo:</label>
              <input type="text" [(ngModel)]="checkInData.model" name="model" required placeholder="Honda Civic" />
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Cor:</label>
              <input type="text" [(ngModel)]="checkInData.color" name="color" required placeholder="Preto" />
            </div>
            <div class="form-group">
              <label>Tipo:</label>
              <select [(ngModel)]="checkInData.type" name="type" required>
                <option value="">Selecione...</option>
                <option value="Motorcycle">Moto</option>
                <option value="Car">Carro</option>
              </select>
            </div>
          </div>
          @if (checkInError()) {
            <p class="error">{{ checkInError() }}</p>
          }
          @if (checkInSuccess()) {
            <p class="success">{{ checkInSuccess() }}</p>
          }
          <button type="submit" [disabled]="checkInForm.invalid || checkInLoading()">
            {{ checkInLoading() ? 'Processando...' : 'Fazer Check-In' }}
          </button>
        </form>
      </div>

      <div class="section">
        <h2>Sessões Ativas</h2>
        @if (sessionsLoading()) {
          <p>Carregando...</p>
        } @else if (sessionsError()) {
          <p class="error">{{ sessionsError() }}</p>
        } @else if (activeSessions().length === 0) {
          <p>Nenhuma sessão ativa no momento.</p>
        } @else {
          <table>
            <thead>
              <tr>
                <th>Placa</th>
                <th>Modelo</th>
                <th>Cor</th>
                <th>Tipo</th>
                <th>Check-In</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              @for (session of activeSessions(); track session.id) {
                <tr>
                  <td>{{ session.plate }}</td>
                  <td>{{ session.model }}</td>
                  <td>{{ session.color }}</td>
                  <td>
                    <span [class]="'type-badge ' + (session.type?.toLowerCase() || '')">
                      {{ session.type === 'Motorcycle' ? 'Moto' : 'Carro' }}
                    </span>
                  </td>
                  <td>{{ formatDate(session.entryTime) }}</td>
                  <td>
                    <button class="btn-preview" (click)="onPreviewCheckOut(session.plate)">
                      Prévia
                    </button>
                    <button class="btn-checkout" (click)="onCheckOut(session.plate)">
                      Check-Out
                    </button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        }
      </div>

      @if (previewData()) {
        <div class="modal">
          <div class="modal-content">
            <h3>Prévia do Check-Out</h3>
            <div class="preview-info">
              <p><strong>Placa:</strong> {{ previewData()!.plate }}</p>
              <p><strong>Modelo:</strong> {{ previewData()!.model }}</p>
              <p><strong>Check-In:</strong> {{ formatDate(previewData()!.entryTime) }}</p>
              <p><strong>Check-Out Previsto:</strong> {{ formatDate(previewData()!.exitTime) }}</p>
              <p><strong>Duração:</strong> {{ previewData()!.timeSpent }}</p>
              <p class="price"><strong>Valor a Pagar:</strong> {{ previewData()!.currency }} {{ previewData()!.totalPrice.toFixed(2) }}</p>
            </div>
            <div class="form-actions">
              <button (click)="confirmCheckOut(previewData()!.plate)">Confirmar Check-Out</button>
              <button (click)="previewData.set(null)">Fechar</button>
            </div>
          </div>
        </div>
      }

      @if (checkOutResult()) {
        <div class="modal">
          <div class="modal-content success-modal">
            <h3>✓ Check-Out Realizado</h3>
            <div class="preview-info">
              <p><strong>Placa:</strong> {{ checkOutResult()!.plate }}</p>
              <p><strong>Modelo:</strong> {{ checkOutResult()!.model }}</p>
              <p><strong>Check-In:</strong> {{ formatDate(checkOutResult()!.entryTime) }}</p>
              <p><strong>Check-Out:</strong> {{ formatDate(checkOutResult()!.exitTime) }}</p>
              <p><strong>Duração:</strong> {{ checkOutResult()!.timeSpent }}</p>
              <p class="price"><strong>Total Pago:</strong> {{ checkOutResult()!.currency }} {{ checkOutResult()!.totalPrice.toFixed(2) }}</p>
            </div>
            <button (click)="closeCheckOutResult()">Fechar</button>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
      max-width: 1200px;
      margin: 0 auto;
    }

    .section {
      background: white;
      padding: 20px;
      border-radius: 8px;
      margin-bottom: 30px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    h2 {
      color: #333;
      margin: 0 0 20px 0;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 15px;
      margin-bottom: 15px;
    }

    .form-group {
      display: flex;
      flex-direction: column;
    }

    label {
      margin-bottom: 5px;
      font-weight: 500;
      color: #555;
    }

    input,
    select {
      padding: 8px 12px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
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
      margin-top: 10px;
    }

    button:hover:not(:disabled) {
      background: #1976D2;
    }

    button:disabled {
      background: #ccc;
      cursor: not-allowed;
    }

    table {
      width: 100%;
      border-collapse: collapse;
      margin-top: 15px;
    }

    thead {
      background: #2196F3;
      color: white;
    }

    th, td {
      padding: 12px;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }

    tbody tr:hover {
      background: #f5f5f5;
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

    .btn-preview {
      padding: 6px 12px;
      font-size: 12px;
      background: #FF9800;
      margin-right: 5px;
    }

    .btn-preview:hover {
      background: #F57C00;
    }

    .btn-checkout {
      padding: 6px 12px;
      font-size: 12px;
      background: #4CAF50;
    }

    .btn-checkout:hover {
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
      max-width: 500px;
      width: 90%;
    }

    .success-modal {
      border-top: 4px solid #4CAF50;
    }

    .preview-info {
      margin: 20px 0;
    }

    .preview-info p {
      margin: 10px 0;
    }

    .price {
      color: #4CAF50;
      font-size: 18px;
      margin-top: 15px;
      padding-top: 15px;
      border-top: 2px solid #eee;
    }

    .form-actions {
      display: flex;
      gap: 10px;
      margin-top: 20px;
    }

    .form-actions button:last-child {
      background: #f5f5f5;
      color: #333;
    }

    .error {
      color: #f44336;
      padding: 10px;
      background: #ffebee;
      border-radius: 4px;
      margin: 10px 0;
    }

    .success {
      color: #4CAF50;
      padding: 10px;
      background: #e8f5e9;
      border-radius: 4px;
      margin: 10px 0;
    }
  `]
})
export class SessionManagementComponent {
  private readonly apiService = inject(ApiService);

  checkInData: CheckInRequest = {
    plate: '',
    model: '',
    color: '',
    type: '' as VehicleType
  };

  activeSessions = signal<Session[]>([]);
  sessionsLoading = signal(true);
  sessionsError = signal('');

  checkInLoading = signal(false);
  checkInError = signal('');
  checkInSuccess = signal('');

  previewData = signal<any>(null);
  checkOutResult = signal<any>(null);

  constructor() {
    this.loadActiveSessions();
  }

  loadActiveSessions(): void {
    this.sessionsLoading.set(true);
    this.apiService.getActiveSessions().subscribe({
      next: (sessions: any[]) => {
        console.log('Sessions received:', sessions);
        
        // Normaliza os dados para camelCase
        const normalizedSessions = sessions.map(s => ({
          id: s.id || s.Id,
          plate: s.plate || s.Plate,
          model: s.model || s.Model,
          color: s.color || s.Color,
          type: s.type || s.Type,
          entryTime: s.entryTime || s.EntryTime || s.checkInTime || s.CheckInTime,
          parkingId: s.parkingId || s.ParkingId
        }));
        
        this.activeSessions.set(normalizedSessions as any);
        this.sessionsLoading.set(false);
      },
      error: (err) => {
        this.sessionsError.set('Erro ao carregar sessões: ' + err.message);
        this.sessionsLoading.set(false);
      }
    });
  }

  onCheckIn(): void {
    this.checkInLoading.set(true);
    this.checkInError.set('');
    this.checkInSuccess.set('');

    this.apiService.checkIn(this.checkInData).subscribe({
      next: () => {
        this.checkInSuccess.set('Check-in realizado com sucesso!');
        this.checkInLoading.set(false);
        this.checkInData = {
          plate: '',
          model: '',
          color: '',
          type: '' as VehicleType
        };
        this.loadActiveSessions();
        setTimeout(() => this.checkInSuccess.set(''), 3000);
      },
      error: (err) => {
        this.checkInError.set('Erro ao fazer check-in');
        this.checkInLoading.set(false);
      }
    });
  }

  onPreviewCheckOut(plate: string): void {
    this.apiService.getCheckOutPreview(plate).subscribe({
      next: (preview: any) => {
        console.log('Preview data received:', preview);
        this.previewData.set(preview);
      },
      error: (err) => {
        console.error('Preview error:', err);
        this.sessionsError.set('Erro ao obter prévia: ' + err.message);
      }
    });
  }

  confirmCheckOut(plate: string): void {
    this.previewData.set(null);
    this.onCheckOut(plate);
  }

  onCheckOut(plate: string): void {
    this.apiService.checkOut({ plate }).subscribe({
      next: (result: any) => {
        console.log('CheckOut result:', result);
        this.checkOutResult.set(result);
        this.loadActiveSessions();
      },
      error: (err) => {
        this.sessionsError.set('Erro ao fazer check-out: ' + err.message);
      }
    });
  }

  closeCheckOutResult(): void {
    this.checkOutResult.set(null);
  }

  formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false
  });
}
// Saída: "22/05/2023 15:30:00"

}
