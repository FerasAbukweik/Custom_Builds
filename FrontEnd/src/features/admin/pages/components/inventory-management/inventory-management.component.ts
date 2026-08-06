import { Component, inject } from '@angular/core';
import { ProductService } from 'src/core/services/client-services/product-service';
import { LoadingComponent } from 'src/shared/components/loading/loading.component';
import { IsVisableDirective } from 'src/shared/directives/is-visable.directive';

@Component({
  selector: 'app-inventory-management',
  imports: [LoadingComponent, IsVisableDirective],
  templateUrl: './inventory-management.component.html',
})
export class InventoryManagementComponent {
  // DI
  protected readonly productsService = inject(ProductService);

  parts = [
    { icon: 'videogame_asset', name: 'PS5 Shell', active: true },
    { icon: 'keyboard', name: '65% PCB Base', active: false },
    { icon: 'cable', name: 'Aviator Cables', active: false },
  ];

  sections = [
    { icon: 'layers', name: 'Faceplate', active: true },
    { icon: 'touchpad_mouse', name: 'Touchpad', active: false },
    { icon: 'switch_access_shortcut', name: 'Triggers (L2/R2)', active: false },
  ];
  mods = [
    { name: 'Matte Black', price: '+$15.00 Premium', colorClass: 'bg-[#000] border-dark-gray', image: 'assets/images/mods/matte-black.jpg' },
    {
      name: 'Iridescent Blue',
      price: '+$25.00 Premium',
      colorClass: 'bg-secondary border-off-white/20 shadow-[0_0_8px_rgba(19,91,236,0.4)]',
      image: 'assets/images/mods/iridescent-blue.jpg'
    },
    {
      name: 'Carbon Fiber',
      price: '+$35.00 Premium',
      colorClass: 'bg-dark-gray border-dark-blue-gray',
    },
  ];
}
