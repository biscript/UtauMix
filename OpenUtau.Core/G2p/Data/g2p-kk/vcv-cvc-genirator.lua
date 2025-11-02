-- phoneme-mapped VCV/CVC generator
-- outputs: <cyrillic_word><two spaces><phoneme_codes separated by spaces>

-- mapping: phoneme code (latin) -> cyrillic character
local phonemap = {
  -- vowels
  ["ae"] = "ә", ["ii"] = "и", ["iu"] = "ү", ["i"] = "і", ["io"] = "ө",
  ["e"]  = "е",  ["a"]  = "а", ["iy"] = "й", ["ou"] = "ұ", ["y"]  = "ы",
  ["o"]  = "о",  ["u"]  = "у",

  -- consonants / digraphs
  ["b"]  = "б", ["g"]  = "г", ["d"]  = "д", ["gh"] = "ғ", ["k"]  = "к",
  ["q"]  = "қ", ["l"]  = "л", ["n"]  = "н", ["r"]  = "р", ["s"]  = "с",
  ["t"]  = "т", ["p"]  = "п", ["m"]  = "м", ["sh"] = "ш", ["j"]  = "ж",
  ["x"]  = "х", ["h"]  = "һ", ["z"]  = "з", ["ng"] = "ң",
}

-- groups for vowels and consonants by harmony (ji = thin, ju = thick, ne = neutral)
local groups = {
  v = {
    ji = { "ae", "e", "iu", "i", "io" },   -- жіңішке (thin)
    ju = { "a", "ou", "y", "o" },          -- жуан (thick)
    ne = { "ii", "u" },                    -- neutral (и, у)
  },
  c = {
    ji = { "g", "k" },                     -- consonants tied to thin vowels
    ju = { "gh", "q" },                    -- consonants tied to thick vowels
    ne = { "b","d","l","n","ng","r","s","t","p","m","sh","j","x","h","z" }, -- neutral
  },
}

-- the same pattern lists as в исходнике (for VCV and for CVC)
local patternsVCV =
{
  { "ne", "ne", "ne" }, { "ne", "ne", "ji" }, { "ne", "ne", "ju" },
  { "ji", "ne", "ne" }, { "ju", "ne", "ne" },
  { "ne", "ji", "ne" }, { "ne", "ju", "ne" },
  { "ji", "ji", "ne" }, { "ju", "ju", "ne" },
  { "ne", "ji", "ji" }, { "ne", "ju", "ju" },
  { "ji", "ne", "ji" }, { "ju", "ne", "ju" },
  { "ji", "ji", "ji" }, { "ju", "ju", "ju" },
}

local patternsCVC =
{
  { "ne", "ne", "ne" }, { "ne", "ne", "ji" }, { "ne", "ne", "ju" },
  { "ji", "ne", "ne" }, { "ju", "ne", "ne" },
  { "ne", "ji", "ne" }, { "ne", "ju", "ne" },
  { "ji", "ji", "ne" }, { "ju","ju","ne" },
  { "ne", "ji", "ji" }, { "ne", "ju", "ju" },
  { "ji", "ne", "ji" }, { "ju", "ne", "ju" },
  { "ji", "ji", "ji" }, { "ju", "ju", "ju" },
}

-- helper: join an array with a separator
local function join(tbl, sep)
  sep = sep or ""
  local s = ""
  for i = 1, #tbl do
    if i > 1 then s = s .. sep end
    s = s .. tbl[i]
  end
  return s
end

-- helper: safe map code -> cyrillic (if missing, keep code itself)
local function code_to_cyr(code)
  return phonemap[code] or code
end

-- print single-phoneme table header (like в примере)
-- iterate phonemap in a deterministic order: we'll print vowels then consonants in groups
local function print_single_map()
  local order = {
    "ae","ii","iu","i","io","e","a","iy","ou","y","o","u",
    "b","d","l","n","r","s","t","p","m","sh","j","x","h","z","ng","g","k","gh","q"
  }
  for _, code in ipairs(order) do
    local cyr = code_to_cyr(code)
    if cyr then
      print(cyr .. "  " .. code)
    end
  end
end

-- generate VCV: left is concatenated cyrillic chars (v1+c1+v2), right is phoneme codes joined by spaces
local function gen_VCV()
  for _, pat in ipairs(patternsVCV) do
    local vg1, cg, vg2 = pat[1], pat[2], pat[3]
    for _, v1code in ipairs(groups.v[vg1]) do
      for _, ccode in ipairs(groups.c[cg]) do
        for _, v2code in ipairs(groups.v[vg2]) do
          local left = code_to_cyr(v1code) .. code_to_cyr(ccode) .. code_to_cyr(v2code)
          local right = v1code .. " " .. ccode .. " " .. v2code
          print(left .. "  " .. right)
        end
      end
    end
  end
end

-- generate CVC: left = c1+v1+c2 (cyrillic), right = codes separated by spaces
local function gen_CVC()
  for _, pat in ipairs(patternsCVC) do
    local c1g, vg, c2g = pat[1], pat[2], pat[3]
    for _, c1code in ipairs(groups.c[c1g]) do
      for _, vcode in ipairs(groups.v[vg]) do
        for _, c2code in ipairs(groups.c[c2g]) do
          local left = code_to_cyr(c1code) .. code_to_cyr(vcode) .. code_to_cyr(c2code)
          local right = c1code .. " " .. vcode .. " " .. c2code
          print(left .. "  " .. right)
        end
      end
    end
  end
end

-- Run: print single-phoneme mapping first, then VCV then CVC
print_single_map()
gen_VCV()
gen_CVC()
