import { Component } from '@angular/core';
import { IOption } from './select-customizer.model';
import { TopNavComponent } from '../../../shared/components/top-nav.component/top-nav.component';
import { ControllerCustomizerComponent } from './components/controller-customizer.component/controller-customizer.component';

@Component({
  selector: 'app-select-customizer',
  imports: [TopNavComponent, ControllerCustomizerComponent],
  templateUrl: './select-customizer.component.html',
})
export class SelectCustomizerComponent {
  options: IOption[] = [
    {
      title: 'Controller',
      subtitle: 'Precision Engineering',
      description:
        'Engineered for professional play with hall-effect triggers and remappable back paddles.',
      imgSrc: 'assets/images/keyboard-image.png',
      goTo: 'controller',
    },
    {
      title: 'Keyboard',
      subtitle: 'Tactile Mastery',
      description:
        'Hot-swappable switches, sound-damped gaskets, and aircraft-grade aluminum casing.',
      imgSrc: 'assets/images/keyboard-image.png',
      goTo: 'keyboard',
    },
  ];
}
