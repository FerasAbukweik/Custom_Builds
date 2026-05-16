import { Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomizerService } from '../../../../customizer.service';
import { CartItemServices } from '../../../../../../../core/services/cart-item-services';
import { IAddCustomBuildDTO } from '../../../../../../../core/DTO/add-custom-build-dto';
import { CustomBuildTypeEnum } from '../../../../../../../core/enums/custom-build-type-enum';
import { ActivatedRoute, Router } from '@angular/router';
import { IPart } from '../../../../../../../core/interfaces/customize-data/customize-data.model';
import { PartServices } from '../../../../../../../core/services/part-services';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'aside[customizerSideBar]',
  imports: [CommonModule],
  templateUrl: './customizer-sidebar.component.html',
  host: {
    class:
      'w-full lg:w-100 bg-primary/95 backdrop-blur-xl border-l border-dark-blue-gray flex flex-col @container',
  },
})
export class CustomizerSidebarComponent implements OnInit {
  // injections
  readonly customizerService = inject(CustomizerService);
  readonly cartItemServices = inject(CartItemServices);
  readonly activatedRoute = inject(ActivatedRoute);
  readonly router = inject(Router);
  readonly partServices = inject(PartServices);
  readonly destroyRef = inject(DestroyRef);

  // data
  readonly selectedProduct = this.customizerService.selectedProduct;

  // signals
  activePartId = signal<string>('');
  customizeData = signal<IPart[]>([]);
  isAddingToCart = signal<boolean>(false);

  // computed
  currentPartSections = computed(
    () => this.customizeData()?.find((part) => part.id === this.activePartId())?.sections ?? [],
  );

  // getters
  get pageType(): CustomBuildTypeEnum{
    return this.activatedRoute.snapshot.data['pageType'];
  }

  ngOnInit() {
    this.partServices.getAllParts().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (data) => {
        this.customizeData.set(data);
        if (data.length > 0) {
          this.activePartId.set(data[0].id);
        }
      },
      error: (error) => {
        //todo: show error message
      },
    });
  }

  setActivePartId(newId: string): void {
    this.activePartId.set(newId);
  }

  addToCart(){
    if(this.isAddingToCart()) return;
    this.isAddingToCart.set(true);


    const newCartItem: IAddCustomBuildDTO = {
      modificationIds: Object.values(this.selectedProduct()).filter((id) => id),
      customBuildType: this.pageType,
    };

    this.cartItemServices.addCustomBuild(newCartItem).subscribe({
      next: () => {
        //todo: show item added to cart with like 3s delay
        this.router.navigate(['/cart']);
      },
      error: (error) => {
        //todo: show error message
      },
    });
  };
}
