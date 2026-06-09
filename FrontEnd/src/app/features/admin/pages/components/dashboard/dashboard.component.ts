import { Component, viewChild } from '@angular/core';

@Component({
  selector: 'app-dashboard.component',
  imports: [],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {
  // fields

  // public
  linePath: string = ''; 

  navItems = [
    { icon: 'dashboard', label: 'Dashboard', active: true },
    { icon: 'shopping_bag', label: 'Orders', active: false },
    { icon: 'inventory_2', label: 'Inventory', active: false },
    { icon: 'group', label: 'Customers', active: false },
    { icon: 'palette', label: 'Design Library', active: false }
  ];

  stats = [
    { 
      title: 'Total Revenue', icon: 'payments', value: '$42,850.12',
      iconColor: 'var(--color-secondary)', trendIcon: 'trending_up', 
      trendText: '+18.4% from last month', trendColor: 'var(--color-ok)' 
    },
    { 
      title: 'Pending Orders', icon: 'pending_actions', value: '24', 
      iconColor: 'var(--color-secondary)', trendIcon: '', 
      trendText: '8 requiring custom assembly', trendColor: 'var(--color-soft-gray)' 
    },
    { 
      title: 'Low Stock Alerts', icon: 'warning', value: '5', 
      iconColor: 'var(--color-error)', trendIcon: '', 
      trendText: 'Keycaps (Neon Pink) & PCB v2', trendColor: 'var(--color-error)' 
    },
    { 
      title: 'Active Designs', icon: 'design_services', value: '142', 
      iconColor: 'var(--color-secondary)', trendIcon: '', 
      trendText: '+12 new user submissions', trendColor: 'var(--color-ok)' 
    }
  ];

  inventory = [
    { name: 'Cherry MX Blue Switches', count: '1,240 units', percent: '85%', isAlert: false },
    { name: 'Gateron Yellow Switches', count: '420 units', percent: '40%', isAlert: false },
    { name: '60% PCB Module v2', count: '12 units', percent: '8%', isAlert: true },
    { name: 'Elite V4 Controller Shell', count: '58 units', percent: '22%', isAlert: false },
    { name: 'Dye-Sub Keycap Sets', count: '310 units', percent: '65%', isAlert: false }
  ];

  orders = [
    { 
      id: '#ORD-9402', initials: 'JS', name: 'Jordan Smith', email: 'jsmith@example.com', 
      product: 'Custom Pro Keyboard (Lubed Gaterons)', date: 'Oct 24, 2023', total: '$289.00', status: 'Processing' 
    },
    { 
      id: '#ORD-9405', initials: 'MT', name: 'Mia Taylor', email: 'mia.t@example.com', 
      product: 'Elite Wireless Controller (Back Paddles)', date: 'Oct 23, 2023', total: '$195.50', status: 'Assembling' 
    },
    { 
      id: '#ORD-9399', initials: 'RK', name: 'Ryan Kim', email: 'rykim@example.com', 
      product: 'Mechanical Switch Sample Kit x3', date: 'Oct 23, 2023', total: '$45.00', status: 'Ready to Ship' 
    }
  ];

  designs = [
    { title: 'Cyberpunk 2077 Custom', author: '@neon_racer', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAcMOXuDP-O7mEQxhpUINrl0piVIou06f8o0a0RlB7xvFu-NzUkUi22A_2dmcxoVhIXJYGTVPg6oxNY_yCYKzE1NqiKANaU6X7wQmZ47zFcacBXORlojj7Ld-bb2TStj0cyfyJUREFPwF5CsEno-NcCOoR-P3ckXRxScBcW7efWfWHLQMEPl265NchD0rQ-oiMHlMdu0J8WB4jk1efOpPfMwt0AMLCXWG2pjquSaKKKTuan4JQ6Gavj-4k-x9hreCtQox1-UBDwlIlY', alt: 'Neon colored mechanical keyboard design' },
    { title: 'Minimalist Oak Series', author: '@woodwork_tech', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuBQ-Y2SX8hyRr62Si-TPbs2Kh6xywK6bjLCxqEsgk04CI3SHr7fsH-ziq72sz91MkkZlcObbTH8qejAOb_msf1mhz5a1y-xts3fDjS2Nx1qGvfPlAt3_CcX-WSexOrnEhcjhJ-Bf-ePeFzdQn-abOkNv-OBs6sS9ywMRr_z5Q_1ny7NmbntT6ScGirCpBxrOd942yBuI9gSZl_kwjGEjNQhghni-bbNE9Nc8dn9WGt4MqAdYJCUw7_Q8Vbhtfk2fahESSW0_bZgnOml', alt: 'Modern minimalist white and wood design' },
    { title: 'Vaporwave Bliss', author: '@retro_vibe', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuBNS5sOITsxbpklJ2eLwbsvt8PJdLdINs2XjgROV_imjB0v7x649ksNDsPPzgypVEm1xeaZQOZQOUrYIXuGHQgiBtwTYzeq-n6kPIsn8Ih5BDsw8uo7b0-YU_aE8p_bbCpnbQMLFyhaL_gwbvXYqVrNdP5SEWcC08O3QeKLD5tOguNB4KV5njdAk2OnNAWpZ83kXcw-u5fo3ICRYt7rUqwC0f0ufexXI_apFt_7LzkkYpNDe9jX9X0Vc9nOuOIe5It4709eyAdeVsIs', alt: 'Retro vaporwave style keycap set layout' },
    { title: 'Carbon Fiber Stealth', author: '@stealth_gamer', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDenfHSRkIC708ojQrnca558VUIHzyxj2Rj47TG7BPCVI9EfX9gHyfyCfYcXEDIAE-WK_ShWa_wt9aAy8n20PqeCEFhfOm5s4WfS4vEreKnMlVi5s3ja6OdYbrQ6sR2WYaHmciIhjTz6cH2cgea4ktUZOQ0_LfkPU7YhW1B3lqPbA-owHiOCM_ZJZrQrQyRwWwzP3VZ3boozyHC3-dxD0MfnvI9MvHjVHKiCbRfCikH88VGR_M0YyuSNvNhWS3OF8yc7A810yeHFOY7', alt: 'Carbon fiber textured pro gaming mouse' }
  ];




  // methods
  ngOnInit(): void {
    const dummyData = [1200, 180, 160, 0, 240, 420, 0];

    this.generateSvgPath(dummyData);
  }

  private generateSvgPath(data: number[]) {
    if (!data || data.length === 0) return;

    const svgWidth = 800;
    const svgHeight = 300;
    const paddingBottom = 40; 
    const chartHeight = svgHeight - paddingBottom;

    const maxVal = Math.max(...data) || 1; 
    const xStep = svgWidth / (data.length - 1);

    const points = data.map((val, i) => {
      const x = i * xStep;
      const y = chartHeight - (val / maxVal) * (chartHeight - 20); 
      return { x, y };
    });

    let lineD = `M ${points[0].x} ${points[0].y}`;
    
    for (let i = 0; i < points.length - 1; i++) {
      const p0 = points[i];
      const p1 = points[i + 1];
      
      const cpX1 = p0.x + xStep / 3;
      const cpY1 = p0.y;
      const cpX2 = p1.x - xStep / 3;
      const cpY2 = p1.y;
      
      lineD += ` C ${cpX1} ${cpY1}, ${cpX2} ${cpY2}, ${p1.x} ${p1.y}`;
    }

    this.linePath = lineD;
  }
}
