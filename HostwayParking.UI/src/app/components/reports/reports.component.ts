import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { RevenueByDay, TopVehicle, OccupancyByHour } from '../../models/report.model';

@Component({
  selector: 'app-reports',
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <h1>Relatórios</h1>

      <!-- Revenue Report -->
      <div class="section">
        <h2>Receita por Dia</h2>
        <div class="form-group">
          <label>Últimos dias:</label>
          <input type="number" [(ngModel)]="revenueDays" min="1" max="365" />
          <button (click)="loadRevenueReport()">Carregar</button>
        </div>

        @if (revenueLoading()) {
          <p>Carregando...</p>
        } @else if (revenueError()) {
          <p class="error">{{ revenueError() }}</p>
        } @else if (revenueData().length > 0) {
          <div class="chart-container">
            <table class="report-table">
              <thead>
                <tr>
                  <th>Data</th>
                  <th>Receita</th>
                </tr>
              </thead>
              <tbody>
                @for (item of revenueData(); track item.date) {
                  <tr>
                    <td>{{ formatDate(item.date) }}</td>
                    <td class="revenue">R$ {{ item.revenue.toFixed(2) }}</td>
                  </tr>
                }
              </tbody>
              <tfoot>
                <tr>
                  <td><strong>Total:</strong></td>
                  <td class="revenue"><strong>R$ {{ getTotalRevenue().toFixed(2) }}</strong></td>
                </tr>
              </tfoot>
            </table>
          </div>
        }
      </div>

      <!-- Top Vehicles Report -->
      <div class="section">
        <h2>Veículos que Mais Permaneceram</h2>
        <div class="date-range">
          <div class="form-group">
            <label>Data Inicial:</label>
            <input type="datetime-local" [(ngModel)]="topVehiclesStart" />
          </div>
          <div class="form-group">
            <label>Data Final:</label>
            <input type="datetime-local" [(ngModel)]="topVehiclesEnd" />
          </div>
          <button (click)="loadTopVehiclesReport()">Carregar</button>
        </div>

        @if (topVehiclesLoading()) {
          <p>Carregando...</p>
        } @else if (topVehiclesError()) {
          <p class="error">{{ topVehiclesError() }}</p>
        } @else if (topVehiclesData().length > 0) {
          <table class="report-table">
            <thead>
              <tr>
                <th>Posição</th>
                <th>Placa</th>
                <th>Modelo</th>
                <th>Tempo Total</th>
                <th>Sessões</th>
              </tr>
            </thead>
            <tbody>
              @for (vehicle of topVehiclesData(); track vehicle.plate; let i = $index) {
                <tr>
                  <td>
                    <span class="position" [class.top3]="i < 3">{{ i + 1 }}º</span>
                  </td>
                  <td>{{ vehicle.plate }}</td>
                  <td>{{ vehicle.model }}</td>
                  <td>{{ vehicle.totalTime }}</td>
                  <td>{{ vehicle.visits }}</td>
                </tr>
              }
            </tbody>
          </table>
        }
      </div>

      <!-- Occupancy Report -->
      <div class="section">
        <h2>Taxa de Ocupação por Hora</h2>
        <div class="date-range">
          <div class="form-group">
            <label>Data Inicial:</label>
            <input type="datetime-local" [(ngModel)]="occupancyStart" />
          </div>
          <div class="form-group">
            <label>Data Final:</label>
            <input type="datetime-local" [(ngModel)]="occupancyEnd" />
          </div>
          <button (click)="loadOccupancyReport()">Carregar</button>
        </div>

        @if (occupancyLoading()) {
          <p>Carregando...</p>
        } @else if (occupancyError()) {
          <p class="error">{{ occupancyError() }}</p>
        } @else if (occupancyData().length > 0) {
          <table class="report-table">
            <thead>
              <tr>
                <th>Horário</th>
                <th>Média de Veículos</th>
                <th>Máximo</th>
                <th>Visual</th>
              </tr>
            </thead>
            <tbody>
              @for (item of occupancyData(); track item.hour) {
                <tr>
                  <td>{{ item.hourRange }}</td>
                  <td>{{ item.averageVehicles.toFixed(2) }}</td>
                  <td>{{ item.maxVehicles }}</td>
                  <td>
                    <div class="occupancy-bar">
                      <div 
                        class="occupancy-fill" 
                        [style.width.%]="item.averageVehicles * 10"
                        [class.high]="item.averageVehicles > 5"
                        [class.medium]="item.averageVehicles > 2 && item.averageVehicles <= 5"
                        [class.low]="item.averageVehicles <= 2"
                      ></div>
                    </div>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        }
      </div>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
      max-width: 1200px;
      margin: 0 auto;
    }

    h1 {
      color: #333;
      margin-bottom: 30px;
    }

    h2 {
      color: #2196F3;
      margin: 0 0 20px 0;
      font-size: 20px;
    }

    .section {
      background: white;
      padding: 25px;
      border-radius: 8px;
      margin-bottom: 30px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .form-group {
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .form-group label {
      font-weight: 500;
      color: #555;
    }

    .form-group input {
      padding: 8px 12px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
    }

    .date-range {
      display: grid;
      grid-template-columns: 1fr 1fr auto;
      gap: 15px;
      align-items: end;
      margin-bottom: 20px;
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
      height: fit-content;
    }

    button:hover {
      background: #1976D2;
    }

    .report-table {
      width: 100%;
      border-collapse: collapse;
      margin-top: 15px;
    }

    .report-table thead {
      background: #f5f5f5;
    }

    .report-table th,
    .report-table td {
      padding: 12px;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }

    .report-table tbody tr:hover {
      background: #fafafa;
    }

    .report-table tfoot {
      background: #f5f5f5;
      font-weight: bold;
    }

    .revenue {
      color: #4CAF50;
      font-weight: 500;
    }

    .position {
      display: inline-block;
      padding: 4px 10px;
      border-radius: 12px;
      background: #e0e0e0;
      font-weight: 500;
    }

    .position.top3 {
      background: #FFD700;
      color: #333;
    }

    .occupancy-bar {
      width: 200px;
      height: 20px;
      background: #f0f0f0;
      border-radius: 10px;
      overflow: hidden;
    }

    .occupancy-fill {
      height: 100%;
      transition: width 0.3s ease;
    }

    .occupancy-fill.low {
      background: #4CAF50;
    }

    .occupancy-fill.medium {
      background: #FF9800;
    }

    .occupancy-fill.high {
      background: #f44336;
    }

    .error {
      color: #f44336;
      padding: 10px;
      background: #ffebee;
      border-radius: 4px;
    }

    .chart-container {
      max-height: 400px;
      overflow-y: auto;
    }
  `]
})
export class ReportsComponent {
  private readonly apiService = inject(ApiService);

  // Revenue Report
  revenueDays = 7;
  revenueData = signal<RevenueByDay[]>([]);
  revenueLoading = signal(false);
  revenueError = signal('');

  // Top Vehicles Report
  topVehiclesStart = this.getDefaultStartDate();
  topVehiclesEnd = this.getDefaultEndDate();
  topVehiclesData = signal<TopVehicle[]>([]);
  topVehiclesLoading = signal(false);
  topVehiclesError = signal('');

  // Occupancy Report
  occupancyStart = this.getDefaultStartDate();
  occupancyEnd = this.getDefaultEndDate();
  occupancyData = signal<OccupancyByHour[]>([]);
  occupancyLoading = signal(false);
  occupancyError = signal('');

  constructor() {
    this.loadRevenueReport();
  }

  loadRevenueReport(): void {
    this.revenueLoading.set(true);
    this.revenueError.set('');
    this.apiService.getRevenueByDay(this.revenueDays).subscribe({
      next: (data: any) => {
        console.log('Revenue data received:', data);
        // A API retorna { items: [...] }, então extrair o array de items
        let dataArray = data?.items || data;
        dataArray = Array.isArray(dataArray) ? dataArray : (dataArray ? [dataArray] : []);
        
        const normalized = dataArray.map((d: any) => ({
          date: d.date || d.Date || '',
          revenue: d.revenue || d.Revenue || 0
        }));
        this.revenueData.set(normalized as any);
        this.revenueLoading.set(false);
      },
      error: (err) => {
        this.revenueError.set('Erro ao carregar relatório: ' + err.message);
        this.revenueLoading.set(false);
      }
    });
  }

  loadTopVehiclesReport(): void {
    this.topVehiclesLoading.set(true);
    this.topVehiclesError.set('');
    const start = new Date(this.topVehiclesStart);
    const end = new Date(this.topVehiclesEnd);
    
    this.apiService.getTopVehiclesByTime(start, end).subscribe({
      next: (data: any) => {
        // A API retorna { items: [...] }, então extrair o array de items
        let dataArray = data?.items || data;
        dataArray = Array.isArray(dataArray) ? dataArray : (dataArray ? [dataArray] : []);
        
        const normalized = dataArray.map((d: any) => ({
          plate: d.plate || d.Plate || '',
          model: d.model || d.Model || '',
          totalTime: d.totalTimeParked || d.TotalTimeParked || d.totalTime || d.TotalTime || '00:00:00',
          visits: d.totalSessions ?? d.TotalSessions ?? d.visits ?? d.Visits ?? 0,
          totalMinutes: d.totalMinutes ?? d.TotalMinutes ?? 0
        }));
        this.topVehiclesData.set(normalized as any);
        this.topVehiclesLoading.set(false);
      },
      error: (err) => {
        this.topVehiclesError.set('Erro ao carregar relatório: ' + err.message);
        this.topVehiclesLoading.set(false);
      }
    });
  }

  loadOccupancyReport(): void {
    this.occupancyLoading.set(true);
    this.occupancyError.set('');
    const start = new Date(this.occupancyStart);
    const end = new Date(this.occupancyEnd);
    
    this.apiService.getOccupancyByHour(start, end).subscribe({
      next: (data: any) => {
        // A API retorna { items: [...] }, então extrair o array de items
        let dataArray = data?.items || data;
        dataArray = Array.isArray(dataArray) ? dataArray : (dataArray ? [dataArray] : []);
        
        const normalized = dataArray.map((d: any) => ({
          hour: d.hour ?? 0,
          hourRange: d.hourRange || d.HourRange || '',
          averageVehicles: d.averageVehicles ?? d.AverageVehicles ?? 0,
          maxVehicles: d.maxVehicles ?? d.MaxVehicles ?? 0,
          // Calcula ocupancy como porcentagem (assumindo capacidade máxima)
          occupancy: d.occupancy ?? d.Occupancy ?? 0
        }));
        this.occupancyData.set(normalized as any);
        this.occupancyLoading.set(false);
      },
      error: (err) => {
        this.occupancyError.set('Erro ao carregar relatório: ' + err.message);
        this.occupancyLoading.set(false);
      }
    });
  }

  getTotalRevenue(): number {
    return this.revenueData().reduce((sum, item) => sum + item.revenue, 0);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR');
  }

  getDefaultStartDate(): string {
    const date = new Date();
    date.setDate(date.getDate() - 7);
    return this.formatDateForInput(date);
  }

  getDefaultEndDate(): string {
    return this.formatDateForInput(new Date());
  }

  formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }
}
