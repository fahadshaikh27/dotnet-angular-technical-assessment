import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './product-form.html',
  styleUrl: './product-form.css'
})
export class ProductFormComponent {

  product = {
    name: '',
    description: '',
    price: 0,
    quantity: 0
  };

  constructor(
    private productService: ProductService
  ) {}

  addProduct(): void {

    if (!this.product.name ||
        !this.product.description ||
        this.product.price <= 0 ||
        this.product.quantity <= 0) {

      alert('Please enter valid product details.');
      return;
    }

    this.productService.addProduct(this.product);

    this.product = {
      name: '',
      description: '',
      price: 0,
      quantity: 0
    };
  }
}