import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OptionalsFormComponent } from './optionals-form.component';

describe('OptionalsFormComponent', () => {
  let component: OptionalsFormComponent;
  let fixture: ComponentFixture<OptionalsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OptionalsFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OptionalsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
