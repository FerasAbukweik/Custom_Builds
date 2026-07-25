import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { IPart } from '../../../../../../../core/interfaces/customize-data.model';
import { PartService } from '../../../../../../../core/services/client-services/part-service';
import { CartItemService } from '../../../../../../../core/services/client-services/cart-item-service-service';
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
  // injections
  protected readonly cartItemServices = inject(CartItemService);
  protected readonly partServices = inject(PartService);

  // input
  currPage = input.required<CustomBuildTypeEnum>();

  // signals
  protected activePartId = signal<string>('');
  protected customizeData = signal<IPart[]>([]);
  protected selectedModifications = signal<Record<string, string>>({});

  // computed
  protected totalPrice = computed<number>(() => {
    const idsSet = new Set(Object.values(this.selectedModifications()));

    return this.customizeData().reduce(
      (sum, p) =>
        sum +
        p.sections.reduce(
          (sum, s) =>
            sum +
            s.modifications.filter((m) => idsSet.has(m.id)).reduce((sum, m) => sum + m.price, 0),
          0,
        ),
      0,
    );
  });
  // computed
  protected currentPartSections = computed(
    () => this.customizeData()?.find((part) => part.id === this.activePartId())?.sections ?? [],
  );

  // getters
  ngOnInit() {
    this.partServices.updateParts();
  }

  // methods

  // add custom build to cart
  addToCart = () => {
    const customBuildData: ICustomBuildAddDTO = {
      modificationIds: Object.values(this.selectedModifications()).filter((id) => id), // to make sure we have id
      customBuildType: this.currPage(),
    };

    this.cartItemServices.addCustomBuild(customBuildData);
  };

  // manage selecting modification
  selectModification = (sectionId: string, productId: string) => {
    this.selectedModifications.update((curr) => {
      const isSelected = curr[sectionId] === productId;

      // if already selected remove it
      const newValue = isSelected ? '' : productId;

      return { ...curr, [sectionId]: newValue };
    });
  };
}
