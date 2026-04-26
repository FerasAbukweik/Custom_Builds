import { Component, input } from '@angular/core';
import { IOption } from '../../select-customizer.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-controller-customizer',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './controller-customizer.component.html',
  host: {
    class:
      'relative flex-1 group overflow-hidden border-b min-[950px]:border-b-0 min-[950px]:border-r border-dark-blue-gray transition-all duration-700 min-[950px]:hover:flex-[1.1] bg-primary/5 min-[950px]:bg-transparent',
  },
})
export class ControllerCustomizerComponent {
  data = input.required<{ option: IOption; idx: number }>();
}
