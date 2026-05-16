import { Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomizerService } from '../../../../customizer.service';
import { CartItemServices } from '../../../../../../../core/services/cart-item-services';
import { IAddCustomBuildDTO } from '../../../../../../../core/DTO/add-custom-build-dto';
import { CustomBuildTypeEnum } from '../../../../../../../core/enums/custom-build-type-enum';
import { ActivatedRoute, Router } from '@angular/router';
import { IPart } from '../../../../../../../core/interfaces/customize-data/customize-data.model';
import { PartServices } from '../../../../../../../core/services/part-services';

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
  readonly customizerService = inject(CustomizerService);
  readonly cartItemServices = inject(CartItemServices);
  readonly activatedRoute = inject(ActivatedRoute);
  readonly router = inject(Router);
  readonly partServices = inject(PartServices);
  readonly destroyRef = inject(DestroyRef);

  readonly selectedProduct = this.customizerService.selectedProduct;

  activePartId = signal<string>('');
  customizeData = signal<IPart[]>([]);

  currentPartSections = computed(
    () => this.customizeData()?.find((part) => part.id === this.activePartId())?.sections ?? [],
  );

  ngOnInit() {
    const sub = this.partServices.getAllParts().subscribe({
      next: (data) => {
        this.customizeData.set(data);
      },
      error: (error) => {
        //todo: show error message
      },
    });

    if (this.customizeData().length <= 0) {
      // toDo: show error message
      console.log('no data');
      return;
    }
    this.activePartId.set(this.customizeData()[0].id!);

    this.destroyRef.onDestroy(() => {
      sub.unsubscribe();
    });
  }

  setActivePartId(newId: string): void {
    this.activePartId.set(newId);
  }

  addToCart = () => {
    const currPage: CustomBuildTypeEnum = this.activatedRoute.snapshot.data['pageType'];

    let currentCustomBuildType!: CustomBuildTypeEnum;
    switch (currPage) {
      case CustomBuildTypeEnum.Controller:
        currentCustomBuildType = CustomBuildTypeEnum.Controller;
        break;
      case CustomBuildTypeEnum.Keyboard:
        currentCustomBuildType = CustomBuildTypeEnum.Keyboard;
        break;
      default:
        //todo: should not happen - throw error
        break;
    }

    const newCartItem: IAddCustomBuildDTO = {
      modificationIds: Object.values(this.selectedProduct()).filter((id) => id),
      customBuildType: currentCustomBuildType,
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
