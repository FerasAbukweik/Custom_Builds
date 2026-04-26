import { Component, ElementRef, ViewChild } from '@angular/core';
import { FooterComponent } from "../../shared/components/footer.component/footer.component";
import { RouterLink } from '@angular/router';
import { projectName } from '../../core/constants/constants';
import { IWhyChooseUs } from './home.model';
import { ProductCardComponent } from "./components/product-card.component/product-card.component";
import { TopNavComponent } from "../../shared/components/top-nav.component/top-nav.component";

@Component({
  selector: 'app-home',
  imports: [FooterComponent, RouterLink, ProductCardComponent, TopNavComponent],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  projectName = projectName;
  
  @ViewChild('designsDiv') designsDiv!: ElementRef<HTMLDivElement>;

  whyChooseUs: IWhyChooseUs[] = [
    {
      id: 1,
      icon: 'fa-solid fa-screwdriver-wrench',
      title: 'Premium Parts',
      description: 'Authentic mechanical switches and double-shot PBT high-grade plastics for the ultimate tactile feel.',
    },
    {
      id: 2,
      icon: 'fa-solid fa-truck-fast',
      title: 'Fast Shipping',
      description: '7-day build turnaround and fully tracked priority international shipping to over 50 countries.',
    },
    {
      id: 3,
      icon: 'fa-solid fa-award',
      title: 'Out Warranty',
      description: '1-month comprehensive coverage on all custom builds. We stand by our artisan craftsmanship.',
    },
  ];

  trendingProducts = new Array(10);

  scroll(direction: 'left' | 'right') {
    const div = this.designsDiv.nativeElement;
    const scrollAmount = 350;
    
    div.scrollBy({
      left: direction === 'left' ? -scrollAmount : scrollAmount,
      behavior: 'smooth',
    });
  }
}