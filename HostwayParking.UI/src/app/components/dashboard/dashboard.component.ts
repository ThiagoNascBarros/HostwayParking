import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  template: `
    <div class="dashboard">
      <h1>Sistema de Gerenciamento de Estacionamento</h1>
      <p class="subtitle">Bem-vindo ao Hostway Parking</p>

      <div class="cards">
        <a routerLink="/sessions" class="card primary">
          <div class="icon">🚗</div>
          <h2>Sessões</h2>
          <p>Check-in e Check-out de veículos</p>
        </a>

        <a routerLink="/vehicles" class="card">
          <div class="icon">📋</div>
          <h2>Veículos</h2>
          <p>Gerenciar veículos cadastrados</p>
        </a>

        <a routerLink="/parkings" class="card">
          <div class="icon">🏢</div>
          <h2>Estacionamentos</h2>
          <p>Visualizar e cadastrar estacionamentos</p>
        </a>

        <a routerLink="/reports" class="card">
          <div class="icon">📊</div>
          <h2>Relatórios</h2>
          <p>Análises e estatísticas</p>
        </a>
      </div>
    </div>
  `,
  styles: [`
    .dashboard {
      padding: 40px 20px;
      max-width: 1200px;
      margin: 0 auto;
    }

    h1 {
      text-align: center;
      color: #2196F3;
      font-size: 36px;
      margin: 0 0 10px 0;
    }

    .subtitle {
      text-align: center;
      color: #666;
      font-size: 18px;
      margin-bottom: 50px;
    }

    .cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 30px;
    }

    .card {
      background: white;
      padding: 40px 30px;
      border-radius: 12px;
      box-shadow: 0 4px 6px rgba(0,0,0,0.1);
      text-align: center;
      transition: transform 0.3s, box-shadow 0.3s;
      cursor: pointer;
      text-decoration: none;
      color: inherit;
    }

    .card:hover {
      transform: translateY(-5px);
      box-shadow: 0 8px 12px rgba(0,0,0,0.15);
    }

    .card.primary {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
    }

    .icon {
      font-size: 48px;
      margin-bottom: 20px;
    }

    .card h2 {
      margin: 0 0 10px 0;
      font-size: 24px;
    }

    .card p {
      margin: 0;
      opacity: 0.8;
    }

    .card.primary p {
      opacity: 0.9;
    }
  `]
})
export class DashboardComponent {}
