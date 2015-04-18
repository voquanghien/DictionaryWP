
function fuzzySearch(t, p) { // returns minimum edit distance between substring of t and p
  var a = [], // current row
      b = [], // previous row
      pa = [], // from
      pb = [],
      s, i, j;
  for (i = 0; i <= p.length; i++) {
    s = b;
    b = a;
    a = s;
    s = pb;
    pb = pa;
    pa = s;
    for (j = 0; j <= t.length; j++) {
      if (i && j) {
        a[j] = a[j - 1] + 1;
        pa[j] = pa[j - 1];
 
        s = b[j - 1] + (t[j - 1] === p[i - 1] ? 0 : 1);
        if (a[j] > s) {
          a[j] = s;
          pa[j] = pb[j - 1];
        }
 
        if (a[j] > b[j] + 1) {
          a[j] = b[j] + 1;
          pa[j] = pb[j];
        }
      } else {
        a[j] = i;
        pa[j] = j;
      }
    }
  }
 
  s = 0;
  for (j = a.length - 1; j >= 1; j--) {
    if (a[j] < a[s]) {
      s = j;
    }
  }
 
  return {
    distance: a[s],
    txt: t.slice(pa[s], s)
  };
}