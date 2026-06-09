import { Component, ElementRef, inject, OnInit, signal, ViewChild } from '@angular/core';
import { FooterComponent } from '../../layouts/footer.component/footer.component';
import { RouterLink } from '@angular/router';
import { projectName } from '../../core/constants/constants';
import { IWhyChooseUs } from './home.model';
import { ProductCardComponent } from './components/product-card.component/product-card.component';
import { TopNavComponent } from '../../layouts/top-nav.component/top-nav.component';
import { HomeService } from './home.service';
import { IProductDTO } from '../../core/DTO/product-dto';
import { IsVisableDirective } from '../../shared/directives/is-visable.directive';
import { LoadingComponent } from '../../shared/components/loading/loading.component';

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
export class HomeComponent implements OnInit {
  // injections
  private readonly _homeService = inject(HomeService);

  // view childs
  @ViewChild('designsDiv') designsDiv!: ElementRef<HTMLDivElement>;

  // signals
  products = this._homeService.getProducts;
  isLoading = this._homeService.getIsLoading;

  // fields
  whyChooseUs = whyChooseUs;
  projectName = projectName;

  ngOnInit(): void {
    // set isMoreDataAvaiable to true so we check for new products each time we revisit the home page
    this._homeService.setIsMoreDataAvaiable(true);
  }

  // methods

  lazyGetData = this._homeService.lazyGetProducts;

  scroll(direction: 'left' | 'right') {
    const div = this.designsDiv.nativeElement;
    const scrollAmount = 350;

    div.scrollBy({
      left: direction === 'left' ? -scrollAmount : scrollAmount,
      behavior: 'smooth',
    });
  }
}
