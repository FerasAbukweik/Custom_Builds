import { Component, ElementRef, inject, OnInit, signal, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { projectName } from '../../core/constants/project';
import { IWhyChooseUs } from './home.model';
import { ProductCardComponent } from './components/product-card.component/product-card.component';
import { FooterComponent } from '../../layouts/footer/footer.component';
import { TopNavComponent } from '../../layouts/top-nav/top-nav.component';
import { IsVisableDirective } from '../../shared/directives/is-visable.directive';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { ProductService } from '../../core/services/client-services/product-service';

const whyChooseUs: IWhyChooseUs[] = [
  {
    id: 1,
    icon: 'fa-solid fa-screwdriver-wrench',
    title: 'Premium Parts',
    description:
      'Authentic mechanical switches and double-shot PBT high-grade plastics for the ultimate tactile feel.',
  },
  {
    id: 2,
    icon: 'fa-solid fa-truck-fast',
    title: 'Fast Shipping',
    description:
      '7-day build turnaround and fully tracked priority international shipping to over 50 countries.',
  },
  {
    id: 3,
    icon: 'fa-solid fa-award',
    title: 'Out Warranty',
    description:
      '1-month comprehensive coverage on all custom builds. We stand by our artisan craftsmanship.',
  },
];

@Component({
  selector: 'app-home',
  imports: [
    FooterComponent,
    RouterLink,
    ProductCardComponent,
    TopNavComponent,
    IsVisableDirective,
    LoadingComponent,
  ],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  // injections
  protected readonly productService = inject(ProductService);

  // view childs
  @ViewChild('designsDiv') designsDiv!: ElementRef<HTMLDivElement>;

  // protected
  protected whyChooseUs = whyChooseUs;
  protected projectName = projectName;

  // methods

  scroll(direction: 'left' | 'right', scrollAmount: number = 350) {
    const div = this.designsDiv.nativeElement;

    div.scrollBy({
      left: direction === 'left' ? -scrollAmount : scrollAmount,
      behavior: 'smooth',
    });
  }
}
