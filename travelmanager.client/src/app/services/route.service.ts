import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  private apiUrl = 'https://localhost:7160/Route';
  constructor(private http: HttpClient) { }

  createRoute(from: string, to: string, price: number): Observable<string> {
    const params = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('price', price.toString());

    return this.http.post(`${this.apiUrl}`, null, { params, responseType: 'text' });
  }

  deleteRoute(from: string, to: string): Observable<string> {
    const params = new HttpParams()
      .set('from', from)
      .set('to', to);

    return this.http.delete(`${this.apiUrl}`, { params, responseType: 'text' });
  }

  updateRoute(routeId: number, from: string, to: string, price: number): Observable<any> {
    const params = new HttpParams()
      .set('routeId', routeId.toString())
      .set('from', from)
      .set('to', to)
      .set('price', price.toString());

    return this.http.patch(`${this.apiUrl}`, null, { params, responseType: 'text' });
  }

  getRoute(from: string, to: string): Observable<string> {
    const params = new HttpParams()
      .set('from', from)
      .set('to', to);

    return this.http.get(this.apiUrl, { params, responseType: 'text' });
  }

  getLowestPrice(from: string, to: string): Observable<string> {
    const params = new HttpParams()
      .set('from', from)
      .set('to', to);

    return this.http.get(`${this.apiUrl}/lowest-price`, { params, responseType: 'text' });
  }
}
