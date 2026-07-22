import { computed, DestroyRef, inject, Injectable, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PartServices } from '../../core/services/api-services/part-services';
import { CartItemServices } from '../../core/services/api-services/cart-item-services';
import { IPart } from '../../core/interfaces/customize-data/customize-data.model';
import { IAddCustomBuildDTO } from '../../core/DTO/add-custom-build-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CustomBuildTypeEnum } from '../../core/enums/custom-build-type-enum';

@Injectable({ providedIn: 'root' })
export class CustomizerService {
  // injections
  private readonly cartItemServices = inject(CartItemServices);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly partServices = inject(PartServices);
  private readonly destroyRef = inject(DestroyRef);

  // signals
  private _activePartId = signal<string>('');
  private _customizeData = signal<IPart[]>([]);
  private _isAddingToCart = signal<boolean>(false);
  private _selectedModifications = signal<Record<string, string>>({});

  // computed
  private _totalPrice = computed<number>(() => {
    const idsSet = new Set(Object.values(this._selectedModifications()));

    return this._customizeData().reduce(
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

  // getters
  get getPageType(): CustomBuildTypeEnum {
    return this.activatedRoute.snapshot.data['pageType'];
  }

  get getActivePartId() {
    return this._activePartId.asReadonly();
  }

  get getCustomizeData() {
    return this._customizeData.asReadonly();
  }

  get getSelectedModifications() {
    return this._selectedModifications.asReadonly();
  }

  get getTotalPrice() {
    return this._totalPrice;
  }

  // setters
  setActivePartId = (id: string) => {
    this._activePartId.set(id);
  };

  // add custom build to cart
  addToCart = () => {
    if (this._isAddingToCart()) return;
    this._isAddingToCart.set(true);

    if (!this.getPageType) {
      // toDo: show error message
      return;
    }

    const newCartItem: IAddCustomBuildDTO = {
      modificationIds: Object.values(this._selectedModifications()).filter((id) => id), // to make sure we have id
      customBuildType: this.getPageType,
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

  // manage selecting modification
  selectModification = (sectionId: string, productId: string) => {
    this._selectedModifications.update((curr) => {
      const isSelected = curr[sectionId] === productId;

      // if already selected remove it
      const newValue = isSelected ? '' : productId;

      return { ...curr, [sectionId]: newValue };
    });
  };

  // get modifications from DB
  updateModifications = () => {
    this.partServices
      .getAllParts()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (data) => {
          this._customizeData.set(data);
          if (data.length > 0) {
            this._activePartId.set(data[0].id);
          }
        },
        error: (error) => {
          //todo: show error message
        },
      });
  };
}
