import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-summary.html',
  styleUrl: './product-summary.css'
})
export class ProductSummaryComponent implements OnInit {

  products: Product[] = [];

  totalProducts = 0;
  totalQuantity = 0;
  totalValue = 0;

  constructor(
    private productService: ProductService
  ) {}

  ngOnInit(): void {

    this.productService.products$
      .subscribe({
        next: (products) => {

          this.products = products;

          this.totalProducts = products.length;

          this.totalQuantity = products.reduce(
            (total, product) => total + product.quantity,
            0
          );

          this.totalValue = products.reduce(
            (total, product) =>
              total + (product.price * product.quantity),
            0
          );
        }
      });

  }
}