import { Component, computed, effect, ElementRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductDialogService } from './product-dialog.service';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { ProductService } from 'src/core/services/client-services/product-service';
import { ProductAddDTO } from 'src/core/DTO/product-add-dto';
import { ProductEditDTO } from 'src/core/DTO/product-edit-dto';

export interface IAddProductFormGroup {
  productName: FormControl<string>;
  price: FormControl<number>;
  inStock: FormControl<number>;
  description: FormControl<string>;
}

@Component({
  selector: 'dialog[app-product-dialog]',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-dialog.component.html',
  styleUrls: ['./product-dialog.component.css'],
  host: {
    class:
      'glass-panel glow-overlay w-[calc(100%-60px)] max-w-2xl rounded-xl border border-dark-gray/40 shadow-2xl p-0 m-auto backdrop:bg-primary/80 backdrop:backdrop-blur-sm open:flex flex-col max-h-[90vh] overflow-scroll text-off-white font-sans',
  },
})
export class ProductDialogComponent implements OnInit {
  // Use the modern inject() function instead of constructor injection
  private dialogRef = inject<ElementRef<HTMLDialogElement>>(ElementRef);

  // DI
  protected readonly productDialogService = inject(ProductDialogService);
  private readonly _productService = inject(ProductService);

  // signals
  protected selectedImagesUrl = computed(() =>
    this.selectedImages().map((f) => URL.createObjectURL(f)),
  );
  protected isEditMode = computed(() => this.productDialogService.product() !== null);

  // private
  private selectedImages = signal<File[]>([]);

  // form
  protected form = new FormGroup<IAddProductFormGroup>({
    productName: new FormControl<string>('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
    price: new FormControl<number>(0, {
      validators: [Validators.required],
      nonNullable: true,
    }),
    inStock: new FormControl<number>(0, {
      validators: [Validators.required],
      nonNullable: true,
    }),
    description: new FormControl<string>('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
  });

  // methods

  reset() {
    this.form.reset();
    this.selectedImages.set([]);
  }

  isError(controlName: keyof IAddProductFormGroup) {
    const control = this.form.controls[controlName];
    if (!control) return false;

    return control.touched && control.invalid;
  }

  selectImages(event: Event) {
    const target = event.target as HTMLInputElement;

    if (!target) return;
    if (!target.files || target.files.length == 0) return;

    const images = Array.from(target.files);

    this.selectedImages.set(images);
  }

  removeImage(imageIdx: number) {
    if (imageIdx >= this.selectedImages().length) return;

    this.selectedImages.update((curr) => curr.filter((_, idx) => idx != imageIdx));
  }

  ngOnInit(): void {
    this.productDialogService.register(this);
  }

  open() {
    this.dialogRef.nativeElement.showModal();

    const product = this.productDialogService.product();
    if (product) {
      this.form.patchValue({
        productName: product.title,
        price: product.price,
        inStock: product.stock,
        description: product.description,
      });
    }
  }

  close() {
    this.dialogRef.nativeElement.close();
    this.reset();
  }

  handleAdd() {
    const formData = this.form.getRawValue();

    const addProductData: ProductAddDTO = {
      name: formData.productName,
      description: formData.description,
      inStock: formData.inStock,
      price: formData.price,
      images: this.selectedImages(),
    };

    this._productService.add(addProductData);
  }

  handleEdit() {
    const formData = this.form.getRawValue();

    const addProductData: ProductEditDTO = {
      id: this.productDialogService.product()!.id,
      name: formData.productName,
      description: formData.description,
      inStock: formData.inStock,
      price: formData.price,
    };

    this._productService.edit(addProductData);
  }

  onSubmit() {
    this.form.markAllAsTouched();

    if (this.form.invalid) return;

    if (this.isEditMode()) {
      this.handleEdit();
    } else {
      this.handleAdd();
    }

    this.close();
  }
}
