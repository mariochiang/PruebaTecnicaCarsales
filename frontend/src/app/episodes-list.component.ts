import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EpisodesService } from './services/episodes.service';
import { Episode } from './models/episode';

@Component({
  selector: 'app-episodes-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="page">
      <h1>Episodes</h1>

      <div class="filters">
        <input placeholder="Search by name..." [(ngModel)]="nameInput" (keyup.enter)="search()"/>
        <button (click)="search()">Search</button>
        <button *ngIf="name()" (click)="clear()">Clear</button>
      </div>

      <div *ngIf="loading()">Loading...</div>
      <div *ngIf="error()" class="error">{{ error() }}</div>

      <table *ngIf="!loading() && items().length" class="grid">
        <thead><tr><th>ID</th><th>Episode</th><th>Name</th><th>Air date</th></tr></thead>
        <tbody>
          <tr *ngFor="let e of items()">
            <td>{{ e.id }}</td>
            <td>{{ e.code }}</td>
            <td>{{ e.name }}</td>
            <td>{{ e.airDate }}</td>
          </tr>
        </tbody>
      </table>

      <div *ngIf="pages() > 1" class="pager">
        <button (click)="prev()" [disabled]="page()<=1">Prev</button>
        <span>Page {{page()}} / {{pages()}}</span>
        <button (click)="next()" [disabled]="page()>=pages()">Next</button>
      </div>
    </div>
  `,
  styles:[`
    .page{padding:16px;max-width:900px;margin:0 auto}
    .filters{display:flex;gap:8px;margin:12px 0}
    .grid{width:100%;border-collapse:collapse}
    .grid th,.grid td{border:1px solid #ccc;padding:6px 8px}
    .pager{display:flex;gap:12px;margin-top:10px}
    .error{color:#b00020;margin:8px 0}
  `]
})
export class EpisodesListComponent {
  page = signal(1);
  pages = signal(1);
  items = signal<Episode[]>([]);
  name = signal('');
  nameInput = '';
  loading = signal(false);
  error = signal<string|null>(null);

  constructor(private api: EpisodesService){ this.load(); }

  private load(){
    this.loading.set(true); this.error.set(null);
    this.api.list(this.page(), this.name()).subscribe({
      next: r => { this.items.set(r.items); this.pages.set(r.pages); this.loading.set(false); },
      error: _ => { this.loading.set(false); this.error.set('Error loading episodes.'); }
    });
  }
  search(){ this.page.set(1); this.name.set(this.nameInput.trim()); this.load(); }
  clear(){ this.nameInput=''; this.name.set(''); this.page.set(1); this.load(); }
  next(){ if(this.page()<this.pages()){ this.page.update(p=>p+1); this.load(); } }
  prev(){ if(this.page()>1){ this.page.update(p=>p-1); this.load(); } }
}
