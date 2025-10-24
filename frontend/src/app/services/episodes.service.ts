import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Episode } from '../models/episode';
import { PaginatedResponse } from '../models/paginated-response';

@Injectable({ providedIn: 'root' })
export class EpisodesService {
  private http = inject(HttpClient);
  private base = `${environment.bffUrl}/api/Episodes`;

  list(page: number, name?: string): Observable<PaginatedResponse<Episode>> {
    let params = new HttpParams().set('page', page);
    if (name?.trim()) params = params.set('name', name.trim());
    return this.http.get<PaginatedResponse<Episode>>(this.base, { params });
  }

  byId(id: number): Observable<Episode> {
    return this.http.get<Episode>(`${this.base}/${id}`);
  }
}
