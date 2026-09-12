import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Product } from '../models/product';
import { AuthService } from './auth';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly apiUrl =
    'https://localhost:7156/api/Product';

  private productsSubject =
    new BehaviorSubject<Product[]>([]);

  public products$ =
    this.productsSubject.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {}

  loadProducts(): void {
    this.http
      .get<Product[]>(
        this.apiUrl,
        { headers: this.getAuthHeaders() }
      )
      .subscribe({
        next: (products) => {
          this.productsSubject.next(products);
        },
        error: (error) => {
          console.error('Error loading products:', error);
        }
      });
  }

  addProduct(
    product: Omit<Product, 'id' | 'createdAt'>
  ): void {

    this.http
      .post<Product>(
        this.apiUrl,
        product,
        { headers: this.getAuthHeaders() }
      )
      .subscribe({
        next: () => {
          this.loadProducts();
        },
        error: (error) => {
          console.error('Error creating product:', error);
        }
      });
  }

  private getAuthHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${this.authService.getToken()}`
    });
  }
}
