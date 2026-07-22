import { Directive, ElementRef, inject, output } from '@angular/core';

@Directive({
  selector: '[appIsVisable]',
  standalone : true,
})
export class IsVisableDirective {
  // injections
  private element = inject(ElementRef);
  
  // outputs
  appIsVisable = output<void>();

  // Intersection Observers
  private observer!: IntersectionObserver;

  ngOnInit() {
    this.observer = new IntersectionObserver(([entry]) => {
      if (entry.isIntersecting) {
        this.appIsVisable.emit(); 
      }
    }, {
      root: null,     
      threshold: 0.1  
    });

    this.observer.observe(this.element.nativeElement);
  }

  ngOnDestroy() {
    this.observer?.disconnect();
  }
}
