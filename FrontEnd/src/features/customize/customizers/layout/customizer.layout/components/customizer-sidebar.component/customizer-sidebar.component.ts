import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { PartService } from '../../../../../../../core/services/client-services/part-service';
import { CartItemService } from '../../../../../../../core/services/client-services/cart-item-service-service';
import { CustomizerSelectionService } from '../../customizer-selection-service';
import { CustomBuildTypeEnum } from '../../../../../../../core/enums/custom-build-type-enum';
import { ICustomBuildAddDTO } from '../../../../../../../core/DTO/add-custom-build-dto';

@Component({
  selector: 'aside[customizerSideBar]',
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './customizer-sidebar.component.html',
  host: {
    class:
      'w-full lg:w-100 bg-primary/95 backdrop-blur-xl border-l border-dark-blue-gray flex flex-col @container',
  },
})
export class CustomizerSidebarComponent implements OnInit {
  // Injections
  protected readonly cartItemServices = inject(CartItemService);
  protected readonly partServices = inject(PartService);
  protected readonly selectionService = inject(CustomizerSelectionService);

  // Input
  currPage = input.required<CustomBuildTypeEnum>();

  // UI-Specific Signals
  protected activePartId = signal<string>('');

  // Computed Properties (Delegated or UI-local)
  protected totalPrice = this.selectionService.totalPrice;
  
  protected currentPartSections = computed(
    () => this.partServices.parts().find((part) => part.id === this.activePartId())?.sections ?? [],
  );

  // Lifecycle
  ngOnInit() {
    this.partServices.updateParts();
  }

  // Methods
  addToCart = () => {
    const customBuildData: ICustomBuildAddDTO = {
      modificationIds: Object.values(this.selectionService.selectedModifications()).filter((id) => id),
      customBuildType: this.currPage(),
    };

    this.cartItemServices.addCustomBuild(customBuildData);
  };

  selectModification = (sectionId: string, modificationId: string) => {
    this.selectionService.selectModification(sectionId, modificationId);
  };
}