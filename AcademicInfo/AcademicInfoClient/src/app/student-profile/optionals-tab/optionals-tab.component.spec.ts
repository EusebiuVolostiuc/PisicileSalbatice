import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OptionalsTabComponent } from './optionals-tab.component';

describe('OptionalsTabComponent', () => {
  let component: OptionalsTabComponent;
  let fixture: ComponentFixture<OptionalsTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OptionalsTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OptionalsTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
