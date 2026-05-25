import { DestroyRef, inject, Injectable, signal } from "@angular/core";
import { ProductService } from "../../core/services/api-services/product-service";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { ILazyLoadingDTO } from "../../core/DTO/lazy-loading-dto";
import { IProductDTO } from "../../core/DTO/product-dto";

@Injectable({providedIn: 'root'})
export class HomeService {
    // injections
    private readonly _productService = inject(ProductService);
    private readonly _destroyRef = inject(DestroyRef);

    // signals
    private _products = signal<IProductDTO[]>([])
    private _isLoading = signal<boolean>(false);

    // fields

    // private
    private readonly _untilDestroyed = takeUntilDestroyed(this._destroyRef);
    private _isMoreDataAvaiable = true;
    private readonly _requestData: ILazyLoadingDTO = {
        ElementsPerSection: 10,
        taken: 0
    }

    // getters
    get getProducts(){
        return this._products.asReadonly();
    }

    get getIsLoading(){
        return this._isLoading.asReadonly();
    }

    // setters
    setIsMoreDataAvaiable(newVal: boolean){
        this._isMoreDataAvaiable = newVal;
    }


    // methods

    // lazyGetProducts
    lazyGetProducts = () => {
        if(this._isLoading() || !this._isMoreDataAvaiable) return;
        this._isLoading.set(true);

        this._productService.getAll(this._requestData).pipe(this._untilDestroyed).subscribe({
            next: (res) => {
                const newProducts = res as IProductDTO[];

                this._products.update(curr => [...curr , ...newProducts]);

                this._requestData.taken += newProducts.length;
                this._isMoreDataAvaiable = newProducts.length > 0;
                this._isLoading.set(false);
            },
            error: (err) => {
                //toDO: show error message
                if(err.status === 404){
                    this._isMoreDataAvaiable = false;
                }

                this._isLoading.set(false);
            }
        })
    }
}