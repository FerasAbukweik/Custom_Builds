import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'footer[appFooter]',
  imports: [CommonModule],
  templateUrl: './footer.component.html',
  host: {
    class: 'text-off-white pt-20 pb-10 px-6 md:px-12 lg:px-20 border-t border-deep-blue',
  },
})
export class FooterComponent {
  socialIcons = ['globe', 'message', 'play'];

  footerSections = [
    {
      title: 'Shop',
      links: ['Controllers', 'Keyboards', 'Switches', 'Keycaps'],
    },
    {
      title: 'Company',
      links: ['About Us', 'Careers', 'Privacy Policy', 'Contact'],
    },
    {
      title: 'Support',
      links: ['FAQ', 'Shipping', 'Warranty', 'Returns'],
    },
  ];

  currentYear = new Date().getFullYear();
}
