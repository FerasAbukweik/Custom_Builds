import { DestroyRef, inject, Injectable, signal } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { PartServices } from "../../core/services/api-services/part-services";
import { CartItemServices } from "../../core/services/api-services/cart-item-services";
import { IPart } from "../../core/interfaces/customize-data/customize-data.model";
import { IAddCustomBuildDTO } from "../../core/DTO/add-custom-build-dto";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { CustomBuildTypeEnum } from "../../core/enums/custom-build-type-enum";

@Injectable({providedIn: 'root'})
export class CustomizerService{
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

    // fields
    private untilDestroyed = takeUntilDestroyed(this.destroyRef);

    // getters
    get getPageType(): CustomBuildTypeEnum {
        return this.activatedRoute.snapshot.data['pageType'];
    }

    get getActivePartId(){
        return this._activePartId.asReadonly();
    }

    get getCustomizeData(){
        return this._customizeData.asReadonly();
    }

    get getSelectedModifications(){
        return this._selectedModifications.asReadonly();
    }


    // setters
    setActivePartId = (id: string) =>{
        this._activePartId.set(id);
    }

    // add custom build to cart
    addToCart = () => {
    if (this._isAddingToCart()) return;
    this._isAddingToCart.set(true);

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
  }


  // manage selecting modification
  selectModification = (sectionId: string, productId: string) => {
    this._selectedModifications.update((curr) => {
      const isSelected = curr[sectionId] === productId;

      // if already selected remove it
      const newValue = isSelected ? '' : productId;

      return { ...curr, [sectionId]: newValue };
    });
  }

  
  // get modifications from DB
  updateModifications = ()=>{
    this.partServices.getAllParts()
      .pipe(this.untilDestroyed)
      .subscribe({
        next: (data) => {
            const data2 : IPart[] = data as IPart[];

            this._customizeData.set(data2);
            if (data2.length > 0) {
                this._activePartId.set(data2[0].id);
            }
        },
        error: (error) => {
          //todo: show error message
        },
    });
  }
}