import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PriceListEditComponent } from './price-list-edit.component';

describe('PriceListEditComponent', () => {
  let component: PriceListEditComponent;
  let fixture: ComponentFixture<PriceListEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PriceListEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PriceListEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
