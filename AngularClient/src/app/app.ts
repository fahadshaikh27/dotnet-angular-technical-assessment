import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductListComponent } from './components/product-list/product-list';
import { ProductSummaryComponent } from './components/product-summary/product-summary';
import { ProductFormComponent } from './components/product-form/product-form';
import { AuthService } from './services/auth';
import { ProductService } from './services/product';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ProductListComponent,
    ProductSummaryComponent,
    ProductFormComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  username = 'fahad';
  password = 'Password@123';
  isLoading = false;
  errorMessage = '';

  constructor(
    public authService: AuthService,
    private productService: ProductService
  ) {}

  login(): void {
    this.errorMessage = '';
    this.isLoading = true;

    this.authService.login(this.username, this.password)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.productService.loadProducts();
        },
        error: () => {
          this.isLoading = false;
          this.errorMessage = 'Invalid username or password.';
        }
      });
  }

  logout(): void {
    this.authService.logout();
  }
}
